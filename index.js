//Express 기본 모듈 불러오기.
var express = require('express');
var http = require('http');
var static = require('serve-static');
var bodyParser = require('body-parser');
var path = require('path');
var mysql      = require('mysql');
var dbconfig   = require('./config/database.js');
var connection = mysql.createConnection(dbconfig);

//Express 객체 생성
var app = express();
//기본포트를 app 객체에 속성으로 설정
app.set('port', process.env.PORT || 3000);

//body-parser를 사용해 application/x-www-form-urlencoded 파싱
app.use(bodyParser.urlencoded({extended: false}));
//body-parser를 사용해 application/json 파싱
app.use(bodyParser.json());

app.use(static(path.join(__dirname, 'public')));

app.get('/', function(req, res){
  res.send('Root');
});

//미들웨어에서 파라미터 확인
app.use('/rank', function(req, res){
  console.log('첫번째 미들웨어에서 요청을 처리함.');

  var id = req.body.id || req.query.id;
  var version = 1;
  var score = req.body.score || req.query.score;
  var deck = req.body.deck || req.query.deck;

  var params = new Array(id, version, score, deck);

  connection.query('Insert into rank_info (id, version, score, deck) values (?, ?, ?, ?) ON DUPLICATE KEY UPDATE score=values(score), deck=values(deck)', params, function(err, rows) {
    if(err) throw err;
    console.log('The result is: ', rows);
  });

  connection.query('SELECT * from rank_info where id = ?', id, function(err, rows) {
    if(err) throw err;

    console.log('The result is: ', rows);
    res.send(rows);
  });  
});

//Express 서버 시작
http.createServer(app).listen(app.get('port'), function(){
  console.log('Express 서버를 시작했습니다. : '+ app.get('port'));
});