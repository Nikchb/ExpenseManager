import getBaseUrl from '../services/api-info'
import axios from 'axios'

const URL = "/category"

export default {
  actions: {    
    async fetchCategories({ commit }) {            
      return await axios({url: getBaseUrl() + URL + '/all', method: 'GET' })
      .then(resp => {         
        return resp.data                 
      })
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      }) 
    },
    async fetchCategoryById({commit, dispatch}, id) {
      return await axios({url: getBaseUrl() + URL + '/'+id, method: 'GET' })
      .then(resp => {         
        return resp.data                 
      })
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })
    },
    async updateCategory({ commit }, { title, limit, id }) {
      return await axios({url: getBaseUrl() + URL, data: { title, limit, id }, method: 'PUT' })
      .then(resp => {
        return resp.data;
      })
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })     
    },
    async createCategory({ commit }, { title, limit }) {     
      return await axios({url: getBaseUrl() + URL, data: { title, limit }, method: 'POST' })
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