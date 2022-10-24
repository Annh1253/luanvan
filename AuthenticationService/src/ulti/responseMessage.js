function formatMessage(data, httpCode) {
    return {
      data,
      httpCode,
    };
  }
  
  module.exports = formatMessage;