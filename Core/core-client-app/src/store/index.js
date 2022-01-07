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
        inputApprovalData: { AccNo: null, VictimNo: null, AppNo: null, BranchId: null, SumMoney: null, ClaimNo: null, Injury: null, BillsData: null, BankData: null, VictimData: null, UserIdLine: null, RefCodeOtp: null, IsEverAuthorize: false, EverAuthorizeMoney: null, EverAuthorizeHosId:null},
        inputUserData: {
            idcardNo: null,
            idcardLaserCode: null,
            prefix: null,
            fname: null,
            lname: null,
            stringDateofBirth: null,
            mobileNo: null,
            lineId: null,
            base64IdCard: null,
            base64Face: null,
            refCodeOtp:null
        },
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
            return state.claimStateData.filter(w => w.stringPt4 === id)[0]
        },
        hosAppGetter: (state) => (id,appNo) => {
            return state.hosAppStateData.filter(w => w.stringAccNo === id && w.appNo === appNo)[0]
        },
        userGetter: (state) => (id) => {
            return state.userStateData.filter(w => w.lineId !== id)
        }
    },
    
})
