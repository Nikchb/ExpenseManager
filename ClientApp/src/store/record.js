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
      async fetchRecords({commit}, { startDate, endDate, categoryId}) {
        return await axios({url: getBaseUrl() + URL + "/all", method: 'POST', data: { startDate, endDate, categoryId} })
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
      }
    }
  }