import Vue from 'vue'
import VueRouter from 'vue-router'
import Advice from "@/views/Advice/Index.vue";
import Accident from "@/views/Accident/Index.vue";
import AccidentCreate from "@/views/Accident/Create.vue"
import Rights from "@/views/Rights/Index.vue";
import RightsHistory from "@/views/Rights/RightsHistory.vue";
import RightsHistoryDetail from "@/views/Rights/RightsHistoryDetail.vue";
import ApprovalCreate from "@/views/Approval/Create.vue";
import ConfirmOTP from "@/views/Approval/ConfirmOTP.vue";
import Approvals from "@/views/Approval/Index.vue";
import ApprovalDetail from "@/views/Approval/Detail.vue";
import ConfirmMoney from "@/views/Approval/ConfirmMoney.vue";
import Ocr from "@/views/Ocr/Index.vue";
import Rating from "@/views/Rating/Index.vue";
import ApprovalEdit from "@/views/Approval/Edit.vue";
import DownloadPDF from "@/views/Approval/DownloadPDF.vue";



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
        path: "/AccidentCreate",
        name: "AccidentCreate",
        component: AccidentCreate,
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
        path: "/RightsHistoryDetail",
        name: "RightsHistoryDetail",
        component: RightsHistoryDetail,
        
    },
    {
        path: "/ApprovalCreate/:id/:type",
        name: "ApprovalCreate",
        component: ApprovalCreate,
    },
    {
        path: "/ConfirmOTP/:id",
        name: "ConfirmOTP",
        component: ConfirmOTP,
    },
    {
        path: "/Approvals/:id",
        name: "Approvals",
        component: Approvals,
    },
    {
        path: "/ApprovalDetail",
        name: "ApprovalDetail",
        component: ApprovalDetail,
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
        path: "/Rating/:id",
        name: "Rating",
        component: Rating,
    },
    {
        path: "/ApprovalEdit/:id",
        name: "ApprovalEdit",
        component: ApprovalEdit,
        props: true
    },
    {
        path: "/DownloadPDF",
        name: "DownloadPDF",
        component: DownloadPDF,
    },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
