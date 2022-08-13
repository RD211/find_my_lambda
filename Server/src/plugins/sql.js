"use strict";

const dataLambda = require("../data")

module.exports = {
    name: "sql",
    version: "1.0.0",
    register: async server => {
        const config = server.app.config.sql
        const client = await dataLambda(server, config)
        server.expose("client", client)
    }
}