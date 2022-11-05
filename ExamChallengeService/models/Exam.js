const mongoose = require('mongoose')
const examSchema = new mongoose.Schema(
    {
        externalId: {type: Number},
        questions: [
            {
                questionId: {type: String}
            }
        ]
    },
    {
        timestamps: true
    }
)

module.exports =  mongoose.model('Exams', examSchema)