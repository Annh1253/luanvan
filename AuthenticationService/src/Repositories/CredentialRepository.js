const Credential = require("../app/models/Credential");
const cryptoJS = require("crypto-js");

class CredentialRepository {
  async updateCredential(message) {
      if (message.Password) {
        console.log("updating..");
        message.Password = cryptoJS.AES.encrypt(
          message.Password,
          process.env.PASS_SECURE
        ).toString();
      }
      let filter = {
        externalId : message.ExternalId
      }
      try {
        let roleList = [];
        message.Roles.forEach(role => {
            roleList.push({
                name: role.Name
            })
        });
  
        let updatedCredentail = await Credential.findOne(filter);
        updatedCredentail.email = message.Email;
        updatedCredentail.password = message.Password;
        updatedCredentail.roles = roleList;
        updatedCredentail.save()
        .then((data) => {
          console.log("Save successfully");
          return data;
        })
        .catch((err) => {
          throw err;
        });
      } catch (err) {
        throw err;
      }
  }

  async deleteCredential(message) {
    try {
      let filter = {
        externalId : message.ExternalId
      }
      const deleted = await Credential.findOneAndDelete(filter);
      console.log("Delete Successfully");
      return deleted;
    } catch (err) {
      throw err;
    }
  }

  get(id) {
    Credential.findById(id)
      .then((credential) => {
        res.status(200).send(credential);
      })
      .catch((err) => {
        throw err;
      });
  }

  getAll() {
    Credential.find({})
      .then((credentails) => {
        return credentails;
      })
      .catch((err) => {
        throw err;
      });
  }

  async checkPass(message) {
    const credential = await Credential.findById(message.id);
    let hashPassword = cryptoJS.AES.decrypt(
      message.password,
      process.env.PASS_SECURE
    );
    const userPassword = JSON.parse(hashPassword.toString(cryptoJS.enc.Utf8));
    if (userPassword.toString() === message.password) {
      return true;
    }
    throw err;
  }

  register(message) {
    console.log("Createing credential : ", message);
    const encryptedPwd = cryptoJS.AES.encrypt(
      message.Password,
      process.env.PASS_SECURE
    ).toString();
    let roleList = [];
    message.Roles.forEach(role => {
        roleList.push({
            name: role.Name
        })
    });
    const newCredential = new Credential({
      email: message.Email,
      password: encryptedPwd,
      externalId: message.ExternalId,
      roles: roleList,
    });
    newCredential
      .save()
      .then((data) => {
        console.log("Save successfully");
        return data;
      })
      .catch((err) => {
        throw err;
      });
  }
}

module.exports = new CredentialRepository();
