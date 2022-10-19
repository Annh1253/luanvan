const router = require('express').Router()
const UserController = require('../app/Controllers/UserController.js')
const {verifyTokenAndAuthorization} = require('../app/middlewares/verifyToken')
router.put('/:id',UserController.put)
router.delete('/:id', UserController.delete)
router.get('/:id',UserController.get)
router.post('/checkPass/:id',UserController.checkPass)
router.get('/',UserController.getAll)

module.exports = router