const User = require('../models/User')
const Credential = require('../models/Credential')

const cryptoJS = require('crypto-js')
const {multipleMongooseToObject,mongooseToObject} = require('../../ulti/mongoose')
const jwt = require('jsonwebtoken')
class AuthController{
    register(req,res,next){
        const encryptedPwd = cryptoJS.AES.encrypt(req.body.password,process.env.PASS_SECURE).toString()
        const newUser = new Credential({
            firstName: req.body.firstName,
            email: req.body.email,
            password: encryptedPwd,
        })
        newUser.save()
            .then((data)=>{
                res.status(200).send(data)
                console.log('Save successfully')})
            .catch((err)=>{res.status(500).send(err)})
    }
     async login(req,res,next){
         const user = await Credential.findOne({email:"string"})
         console.log(user)
         Credential.findOne({email: req.body.email})
            .then((user) => {
                const newUser = mongooseToObject(user)
                let hashPassword=  cryptoJS.AES.decrypt(user.password,process.env.PASS_SECURE)
                const userPassword =  hashPassword.toString(cryptoJS.enc.Utf8)
                let roleList = [];
                user.roles.forEach(role => {
                    roleList.push(role.name)
                })
                if(userPassword == req.body.password)
                {
                    const accessToken = jwt.sign({
                        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": roleList  
                    },
                        process.env.JWT_SECURE,
                        {expiresIn: "1d"}
                    )
                    const {password,...rest} = user._doc
                    res.status(200).send({...rest,accessToken})
                }else{
                    res.status(401).send("Wrong password")
                }
            })
            .catch(err=>{
                res.status(401).send("Wrong Account Name")
            })
    }
}

module.exports = new AuthController