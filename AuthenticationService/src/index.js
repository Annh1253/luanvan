const express = require('express')
const app = express()
const mongoose = require('mongoose')
const db = require('./config/db')
const route = require('./routes')
const port = 3001
const cors = require('cors')
const messageBusSubscriber = require('./AsyncDataService/MessageBusSubscriber.js');
app.use(cors())

//convert request body to JSON.
app.use(express.urlencoded({
    limit: '50mb',
    extended:true
}))
app.use(express.json({ limit: '50mb' }))

//Connect to Database
db.connect()

//Route APP
route(app)

app.listen(port || 6000, ()=>{
    console.log('Backend server is running at port: ' + port)
})