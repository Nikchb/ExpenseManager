import getBaseUrl from '../services/api-info'
import axios from 'axios'

const URL = "/category"

export default {
  actions: {    
    async fetchCategories({ commit }) {
      let token = localStorage.getItem('token')
      if(token){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token 
      }              
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
    async updateCategory({ commit }, { title, limit, id }) {      
      let token = localStorage.getItem('token')
      if(token){
          axios.defaults.headers.common['Authorization'] = 'Bearer ' + token 
      }
      await axios({url: getBaseUrl() + URL, data: { title, limit, id }, method: 'PUT' })
      .then(resp => {})
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })     
    },
    async createCategory({ commit }, { title, limit }) {
      let token = localStorage.getItem('token')
      if(token){
          axios.defaults.headers.common['Authorization'] = 'Bearer ' + token 
      }
      await axios({url: getBaseUrl() + URL, data: { title, limit }, method: 'POST' })
      .then(resp => {})
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })
    }
  }
}