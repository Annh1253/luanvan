function route(app){

    const examChallengeRouter = require('./ExamChallengeRoute.js')
    
   
    app.use('/api/examchallenge',examChallengeRouter)
  
}

module.exports = route