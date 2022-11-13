const ExamChallengeRepository = require('../repositories/ExamChallengeRepository');
const EventType = require("./EventType")


const addExam = (message) =>
    {
        try{
            ExamChallengeRepository.saveExam(message);
        }catch(err)
        {
            throw err
        }
    }

const addQuestion = (message) =>
    {
        try{
            ExamChallengeRepository.saveQuestion(message);
        }catch(err)
        {
            throw err
        }
    }

    const updateQuestion = (message) =>
    {
        try{
            ExamChallengeRepository.updateQuestion(message);
        }catch(err)
        {
            throw err
        }
    }

const deleteQuestion = (message) => 
    {
        try{
            ExamChallengeRepository.deleteQuestion(message);
        }catch(err)
        {
            throw err
        }
    }
class EventProcessor
{
    

    processEvent(message)
    {
        let eventType = message.Event
        switch(eventType)
        {
            case EventType.NewExamCreate:
                console.log("process create exam event");
                console.log(message);
                addExam(message)
                break;
            case EventType.NewOptionCreate:
                console.log("process create option event");  
                updateQuestion(message)
                break;
            case EventType.NewQuestionCreate:
                console.log("process create question event");
                console.log(message);
                addQuestion(message) 
                break;
            case EventType.UpdateQuestion:
                updateQuestion(message)
                console.log(message);
                console.log("process udpate question event");
                break;
            case EventType.DeleteOption:
                console.log("process delete option event");
                updateQuestion(message)
                break;
            case EventType.DeleteQuestion:
                console.log("process delete question event");
                deleteQuestion(message)
                break;
            default:
                console.log("undefined event");
                break;
        }
    }

    
}

module.exports = new EventProcessor