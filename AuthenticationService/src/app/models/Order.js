const mongoose = require('mongoose')
const OrderSchema = new mongoose.Schema(
    {
        userId: {type:String , required: true},
        products : [
            {
                productId : {type: String},
                size: {type: String},
                additionId: {type: String},
                quanlity: {type: Number, default:1}
            }
        ],
       // amount: {type:Number, required:true},
        deliveryAddress: {type:String, required:true},
        isPayed: {type: Boolean, default: false},
        payMethod: {type: String, default: 'cash'},
        status: {type: String, enum: ['chưa xác nhận', 'chuẩn bị giao', 'đang giao', 'đã giao'], default: "pending"},
        deliveryStaffId: {type: String},
        saleStaffId: {type: String},
        phoneNumber: {type: String}
    },
    {
        timestamps: true
    }
)

module.exports =  mongoose.model('Order',OrderSchema)