const mongoose = require('mongoose')
const userSchema = new mongoose.Schema(
    {
        email: {type: String},
        password: {type: String},
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

module.exports =  mongoose.model('Users',userSchema)