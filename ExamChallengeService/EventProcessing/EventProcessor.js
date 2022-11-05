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

const deleteCredential = (message) => 
    {
        try{
            ExamChallengeRepository.deleteCredential(message);
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
                addExam(message)
                break;
            case EventType.UpdateExam:
                console.log("process update exam event");  
                 
                break;
            case EventType.NewQuestionCreate:
                console.log("process create question event");
                addQuestion(message) 
                break;
            case EventType.NewOptionCreate:
                console.log("process create option event");
                break;
            default:
                console.log("undefined event");
                break;
        }
    }

    
}

module.exports = new EventProcessor