import getBaseUrl from '../services/api-info'
import axios from 'axios'

const URL = "/auth"

export default { 
  state: {    
    token: undefined
  },
  mutations: { 
    init(state){
      let token = localStorage.getItem('token')
      if(token){
        state.token = token
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token
      }
    },   
    auth_success(state, token){      
      state.token = token
      axios.defaults.headers.common['Authorization'] = 'Bearer ' + token   
      localStorage.setItem('token', token)   
    },
    auth_error(state){
      state.token = undefined
      axios.defaults.headers.common['Authorization'] = ''
      localStorage.setItem('token','')
    },
    logout(state){     
      state.token = undefined
      axios.defaults.headers.common['Authorization'] = ''
      localStorage.setItem('token','')
    },
  },
  getters : {    
    authorized: state =>  state.token ? true : false,
  },
  actions: {
    async init({ commit}){
      commit('init')
      await axios({url: getBaseUrl() + '/ping/authorize', method: 'GET'})
      .then(resp => {})
      .catch(err => {
        commit('auth_error')       
      })
    },
    async login({ commit }, { email, password }) {
      await axios({url: getBaseUrl() + URL + "/sign-in", data: { email, password }, method: 'POST' })
        .then(resp => {
          commit('auth_success', resp.data)          
        })
        .catch(err => {
          commit('auth_error')          
        }) 
    },
    async register({ commit }, {email, password, name}) {
      await axios({url: getBaseUrl() + URL + "/sign-up", data: { email, password, name }, method: 'POST' })
        .then(resp => {
          commit('auth_success', resp.data)          
        })
        .catch(err => {
          commit('auth_error')          
        })
    },   
    async logout({commit}) {
      commit('logout')
    }
  }
}
