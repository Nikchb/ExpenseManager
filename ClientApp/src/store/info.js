import getBaseUrl from '../services/api-info'
import axios from 'axios'

const URL = "/user"

export default {
  state: {
    info: {}
  },
  mutations: {
    setInfo(state, info) {
      state.info = info
    },
    clearInfo(state) {
      state.info = {}
    }
  },
  actions: {
    async fetchInfo({commit}) {      
      await axios({url: getBaseUrl() + URL, method: 'GET' })
      .then(resp => {
        commit('setInfo', resp.data)
      })
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })       
    },
    async updateInfo({commit}, {name, lang}) {      
      await axios({url: getBaseUrl() + URL, method: 'PUT', data : {name, lang} })
      .then(resp => {
        commit('setInfo', resp.data)
      })
      .catch(err => {
        if(err.status === 401){
          commit('auth_error')
        }
      })       
    }
  }, 
  getters: {
    info: s => s.info
  }
}