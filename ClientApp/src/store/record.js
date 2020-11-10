import getBaseUrl from '../services/api-info'
import axios from 'axios'

const URL = "/record"

export default {
    actions: {
      async createRecord({commit}, {categoryId, amount, description, isIncome}) {
        return await axios({url: getBaseUrl() + URL, method: 'POST', data: {categoryId, amount, description, isIncome} })
        .then(resp => {
            return resp.data;
        })
        .catch(err => {
            if(err.status === 401){
                commit('auth_error')
            }
        })
      }, 
      async fetchRecordsByPeriod({commit}, { startDate, endDate}) {
        return await axios({url: getBaseUrl() + URL + "/all", method: 'POST', data: { startDate, endDate } })
        .then(resp => {
            return resp.data;
        })
        .catch(err => {
            if(err.status === 401){
                commit('auth_error')
            }
        })
      },
      async fetchRecords({commit}) {
        return await axios({url: getBaseUrl() + URL + "/all", method: 'POST', data: {} })
        .then(resp => {
            return resp.data;
        })
        .catch(err => {
            if(err.status === 401){
                commit('auth_error')
            }
        })
      },
      async fetchRecordById({commit}, id) {
        return await axios({url: getBaseUrl() + URL + "/" + id, method: 'GET' })
        .then(resp => {
            return resp.data;
        })
        .catch(err => {
            if(err.status === 401){
                commit('auth_error')
            }
        })
      }
    }
  }