const User = require('../models/User')
const cryptoJS = require('crypto-js')

class UserController{

    async put(req,res,next){
        console.log('aaa');
        if(req.body.password)
        {
            console.log(req.body.password)
            req.body.password = cryptoJS.AES.encrypt(req.body.password,process.env.PASS_SECURE).toString()
        }
        try{
            const updatedUser = await User.findByIdAndUpdate(req.params.id,req.body,{new:true})
            res.status(200).send(updatedUser)
        }
        catch(err)
        {
            res.status(500).send(err)
        }
    }

    async delete(req,res,next){
        try{
          const deleted = await User.findByIdAndDelete(req.params.id)  
          res.status(200).send(deleted)
        }catch(err){
            res.status(500).send(err)            
        }
    }

    get(req,res,next){
       User.findById(req.params.id)
        .then(user=>{
            res.status(200).send(user)
        })
        .catch(err=>{res.status(500).send(err)})
    }

    getAll(req,res,next){
        User.find({})
            .then(users=>{
                res.status(200).send(users)
            })
            .catch(err=>{res.status(500).send(err)})
    }

   async checkPass(req,res,next){
       const user = await User.findById(req.params.id)
       let hashPassword=  cryptoJS.AES.decrypt(user.password,process.env.PASS_SECURE)
       const userPassword =  JSON.parse(hashPassword.toString(cryptoJS.enc.Utf8))
       if(userPassword.toString() === req.body.password)
       {
           res.status(200).send(true)
       }else{
           res.status(401).send(false)
       }
   }
}

module.exports = new UserController