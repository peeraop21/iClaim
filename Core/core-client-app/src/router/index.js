import Vue from 'vue'
import VueRouter from 'vue-router'
import Advice from "@/components/Advice/Index.vue";
import Accident from "@/components/Accident/Index.vue";
import Rights from "@/components/Rights/Index.vue";
import RightsHistory from "@/components/Rights/RightsHistory.vue";
import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";
import ApprovalCreate from "@/components/Approval/Create.vue";
import ConfirmOTP from "@/components/Approval/ConfirmOTP.vue";
import Approvals from "@/components/Approval/Index.vue";
import ApprovalDetail from "@/components/Approval/Detail.vue";
import ConfirmMoney from "@/components/Approval/ConfirmMoney.vue";
import Ocr from "@/components/Ocr/Index.vue";
import Rating from "@/components/Rating/Index.vue";
import ApprovalEdit from "@/components/Approval/Edit.vue";
import DownloadPDF from "@/components/Approval/DownloadPDF.vue";

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
        path: "/DownloadPDF/:accNo/:victimNo/:appNo/:userIdCard",
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
