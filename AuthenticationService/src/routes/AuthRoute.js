const router = require('express').Router()
const AuthController = require('../app/Controllers/AuthController')

router.post("/register",AuthController.register)
router.post("/login",AuthController.login)

module.exports = router