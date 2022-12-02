const mongoose = require("mongoose");
async function connect() {
  await mongoose.connect(process.env.MONGO_URL_K8S,{
  // await mongoose.connect(process.env.MONGO_URL_LOCAL, {
      useNewUrlParser: true,
      useUnifiedTopology: true,
    })
    .then(() => {
      console.log("Connect successful");
    })
    .catch((err) => {
      console.log(err);
    });
}

module.exports = { connect };
