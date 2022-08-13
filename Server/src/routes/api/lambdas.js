"use strict";

module.exports.register = async server => {
    server.route({
        method: "GET",
        path: "/api/lambdas",
        handler: async request => {
            try {
                const db = request.server.plugins.sql.client

                const lambdaId = 1
                const res = await db.lambdas.getLambda(lambdaId)

                return res.recordset
            } catch (err) {
                console.log(err)
            }
        }
    })
}