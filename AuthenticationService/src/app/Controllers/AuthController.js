const User = require("../models/User");
const Credential = require("../models/Credential");
const EventType = require("../../EventProcessing/EventType");
const cryptoJS = require("crypto-js");
const {
  multipleMongooseToObject,
  mongooseToObject,
} = require("../../ulti/mongoose");
const jwt = require("jsonwebtoken");
const DEFAULT_ROLE = "Contestant";
const messagePublisher = require("../../AsyncDataService/MessageBusClient");
const { credentials } = require("amqplib");

class AuthController {
  async register(req, res, next) {
    let credential = await Credential.findOne({ email: req.body.email });
    if (credential != null) {
      res.status(500).send({
        email: "Email is already registered",
      });
      return;
    }

    const encryptedPwd = cryptoJS.AES.encrypt(
      req.body.password,
      process.env.PASS_SECURE
    ).toString();
    const newUser = new Credential({
      firstName: req.body.firstName,
      email: req.body.email,
      roles: [{ name: DEFAULT_ROLE }],
      password: encryptedPwd,
    });
    newUser.save().then((data) => {
      let roleList = [];
      data.roles.forEach((role) => {
        roleList.push(role.name);
      });
      let newCredential = {
        Email: data.email,
        Password: data.password,
        Roles: roleList,
        Event: EventType.NewCredentialRegisted,
      };

      const accessToken = jwt.sign(
        {
          "email": data.email
        },
        process.env.JWT_SECURE,
        { expiresIn: "1d" }
      );

      try {
        messagePublisher.publishMessage(newCredential);
      } catch (ex) {
        res.status(500).send(ex);
        return;
      }
      res.status(200).send({ accessToken });
    })
      .catch((err) => {
        res.status(500).send(err);
      });

  }

  async login(req, res, next) {
    Credential.findOne({ email: req.body.email })
      .then((user) => {
        const newUser = mongooseToObject(user);
        let hashPassword = cryptoJS.AES.decrypt(
          user.password,
          process.env.PASS_SECURE
        );
        const userPassword = hashPassword.toString(cryptoJS.enc.Utf8);
        if (userPassword == req.body.password) {
          const accessToken = jwt.sign(
            {
              email: user.email
            },
            process.env.JWT_SECURE,
            { expiresIn: "1d" }
          );
          const { password, ...rest } = user._doc;
          res.status(200).send({ ...rest, accessToken });
        } else {
          res.status(401).send({
            password: "Wrong password",
          });
        }
      })
      .catch((err) => {
        res.status(401).send({
          email: "Wrong email",
        });
      });
  }
}

module.exports = new AuthController();
