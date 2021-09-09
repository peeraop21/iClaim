import Vue from 'vue'
import VueRouter from 'vue-router'
import Advice from "@/components/Advice/Index.vue";
import Accident from "@/components/Accident/Index.vue";
import Rights from "@/components/Rights/Index.vue";
import RightsHistory from "@/components/Rights/RightsHistory.vue";
import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";
import Claim from "@/components/Claim/Claim.vue";
import Bill from "@/components/Claim/Bill.vue";
import Bookbank from "@/components/Claim/Bookbank.vue";
import Preview from "@/components/Claim/Preview.vue";
import ConfirmOTP from "@/components/Claim/ConfirmOTP.vue";
import CheckStatus from "@/components/Status/CheckStatus.vue";
import ClaimDetail from "@/components/Status/ClaimDetail.vue";
import ConfirmMoney from "@/components/Status/ConfirmMoney.vue";
import Ocr from "@/components/Ocr/Index.vue";
import Rating from "@/components/Rating/Index.vue";
import AddDocument from "@/components/Claim/AddDocument.vue";

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
        path: "/Rights/:id",
        name: "Rights",
        component: Rights,
    },
    {
        path: "/RightsHistory/:id/:typerights",
        name: "RightsHistory",
        component: RightsHistory,
    },
    {
        path: "/RightsHistoryDetail/:id/:typerights/:pt/:typept",
        name: "RightsHistoryDetail",
        component: RightsHistoryDetail,
        
    },
    {
        path: "/Claim/:id/:type",
        name: "Claim",
        component: Claim,
    },
    {
        path: "/Bill",
        name: "Bill",
        component: Bill,
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
        path: "/ConfirmOTP/:id",
        name: "ConfirmOTP",
        component: ConfirmOTP,
    },
    {
        path: "/CheckStatus/:id",
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
    {
        path: "/Ocr",
        name: "Ocr",
        component: Ocr,
    },
    {
        path: "/Rating",
        name: "Rating",
        component: Rating,
    },
    {
        path: "/AddDocument/:id",
        name: "AddDocument",
        component: AddDocument,
    },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
