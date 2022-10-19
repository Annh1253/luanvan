const mongoose = require('mongoose')
const dotenv = require('dotenv')

dotenv.config()

async function connect(){
    await mongoose.connect(process.env.MONGO_URL_KUBERNETE,{
        useNewUrlParser:true,
        useUnifiedTopology:true,
    }).then(()=>{
        console.log('Connect successful')
    }).catch((err)=>{console.log(err)})
}

module.exports = {connect}