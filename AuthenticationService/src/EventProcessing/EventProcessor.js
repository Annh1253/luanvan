const CredentialRepository = require('../Repositories/CredentialRepository');

const EventType = {
    NewUserCreate: 'NewUserCreate'
};

class EventProcessor
{
    addPlatform(message)
    {
        try{
            CredentialRepository.register(message);
        }catch(err)
        {
            throw err
        }
    }

    processEvent(message)
    {
        let eventType = message.Event
        switch(eventType)
        {
            case EventType.NewUserCreate:
                console.log("process create credential event");
                try{
                    CredentialRepository.register(message);
                }catch(err)
                {
                    throw err
                }
                break;
            default:
                break;
        }
    }

    
}

module.exports = new EventProcessor