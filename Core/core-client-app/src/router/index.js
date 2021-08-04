import Vue from 'vue'
import VueRouter from 'vue-router'
import Advice from "@/components/Advice/Index.vue";
import Accident from "@/components/Accident/Index.vue";
import Rights from "@/components/Rights/Index.vue";
import RightsHistory from "@/components/Rights/RightsHistory.vue";
import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";
import Claim from "@/components/Claim/Claim.vue";
import Bookbank from "@/components/Claim/Bookbank.vue";
import Preview from "@/components/Claim/Preview.vue";
import ConfirmOTP from "@/components/Claim/ConfirmOTP.vue";
import CheckStatus from "@/components/Status/CheckStatus.vue";
import ClaimDetail from "@/components/Status/ClaimDetail.vue";
import ConfirmMoney from "@/components/Status/ConfirmMoney.vue";

Vue.use(VueRouter)

const routes = [
    {
        path: "/",
        name: "Advice",
        component: Advice,
    },
    {
        path: "/Accident",
        name: "Accident",
        component: Accident,
    },
    {
        path: "/Rights",
        name: "Rights",
        component: Rights,
    },
    {
        path: "/RightsHistory",
        name: "RightsHistory",
        component: RightsHistory,
    },
    {
        path: "/RightsHistoryDetail",
        name: "RightsHistoryDetail",
        component: RightsHistoryDetail,
    },
    {
        path: "/Claim",
        name: "Claim",
        component: Claim,
    },
    
    {
        path: "/Bookbank",
        name: "Bookbank",
        component: Bookbank,
    },
    {
        path: "/Preview",
        name: "Preview",
        component: Preview,
    },

    {
        path: "/ConfirmOTP",
        name: "ConfirmOTP",
        component: ConfirmOTP,
    },
    {
        path: "/CheckStatus",
        name: "CheckStatus",
        component: CheckStatus,
    },
    {
        path: "/ClaimDetail",
        name: "ClaimDetail",
        component: ClaimDetail,
    },
    {
        path: "/ConfirmMoney",
        name: "ConfirmMoney",
        component: ConfirmMoney,
    },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
