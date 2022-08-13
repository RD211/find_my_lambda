"use strict";

const utils = require("../utils")
const register = async ({sql, getConnection}) => {
    const sqlQueries = await utils.loadSqlQueries("lambdas")
    const getLambda = async lambdaId => {
        const cnx = await getConnection()
        const request = await cnx.request()
        request.input( "lambdaId", sql.Int, lambdaId)
        return request.query(sqlQueries.getLambda)
    }

    return {
        getLambda
    }
}

module.exports = { register }