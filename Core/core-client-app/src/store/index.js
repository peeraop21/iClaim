import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        accStateData: [],
        userStateData: [],
        claimStateData: [],
        hosAppStateData: [],
        userTokenLine:null,
        jwtToken: [],
        inputApprovalData: { AccNo: null, VictimNo: null, AppNo: null, SumMoney: null, ClaimNo: null, Injury: null, BillsData: null, BankData: null, VictimData: null, UserIdLine: null, RefCodeOtp:null},
        hasRegistered: false,
        envUrl:null

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
        ptGetter: (state) => (id) => {
            console.log("Getter", state.claimStateData)
            return state.claimStateData.filter(w => w.stringPt4 === id)[0]
        },
        hosAppGetter: (state) => (id,appNo) => {
            console.log("HosGetter", state.hosAppStateData)
            return state.hosAppStateData.filter(w => w.stringAccNo === id && w.appNo === appNo)[0]
        },
        userGetter: (state) => (id) => {
            return state.userStateData.filter(w => w.lineId !== id)
        }
    },
    
})
