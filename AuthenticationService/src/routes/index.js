function route(app){
    const userRouter = require('./UserRoute.js')
    const authRouter = require('./AuthRoute.js')
    
    app.use('/user',userRouter)
    app.use('/auth',authRouter)
  
}

module.exports = route