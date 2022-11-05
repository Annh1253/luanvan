const mongoose = require('mongoose')
const questionSchema = new mongoose.Schema(
    {
        externalId: {type: Number},
        externalCorrectOptionId: {type: Number}
    },
    {
        timestamps: true
    }
)

module.exports =  mongoose.model('Questions', questionSchema)