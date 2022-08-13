"use strict";

const lambdas = require("./lambdas")

module.exports.register = async server => {
    await lambdas.register(server)
}