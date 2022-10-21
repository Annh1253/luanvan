const mongoose = require('mongoose')
const credentailSchema = new mongoose.Schema(
    {
        email: {type: String},
        password: {type: String},
        externalId: {type: Number},
        roles: [
            {
                name: {type: String}
            }
        ]
    },
    {
        timestamps: true
    }
)

module.exports =  mongoose.model('Credentails', credentailSchema)