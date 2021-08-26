import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        accStateData: [],
        userStateData: [],
        jwtToken:[]

  },
  mutations: {
  },
  actions: {
  },
  modules: {
    },
    getters: {

        accGetter: (state) => (id) => {
            return state.accStateData.filter(w => w.stringAccNo === id)[0]
        },
        userGetter: (state) => (id) => {
            return state.userStateData.filter(w => w.lineId !== id)
        }
    },
    
})
