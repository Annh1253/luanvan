const CredentialRepository = require('../Repositories/CredentialRepository');
const EventType = require("./EventType")


const addCredential = (message) =>
    {
        try{
            CredentialRepository.register(message);
        }catch(err)
        {
            throw err
        }
    }

const updateCredential = (message) =>
    {
        try{
            CredentialRepository.updateCredential(message);
        }catch(err)
        {
            throw err
        }
    }

const deleteCredential = (message) => 
    {
        try{
            CredentialRepository.deleteCredential(message);
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
            case EventType.NewUserCreate:
                console.log("process create credential event");
                addCredential(message)
                break;
            case EventType.UpdateUserCredential:
                console.log("process update credential event");
                updateCredential(message)
                break;
            case EventType.DeleteUser:
                console.log("process delete credential event");
                deleteCredential(message)
                break;
            default:
                console.log("undefined event");
                break;
        }
    }

    
}

module.exports = new EventProcessor