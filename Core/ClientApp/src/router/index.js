import { createWebHistory, createRouter } from "vue-router";
//import Home from "@/components/Home/Home.vue";
import Counter from "@/components/Counter.vue";
import FetchData from "@/components/FetchData.vue";
import Advice from "@/components/Advice/Index.vue";
import Accident from "@/components/Accident/Index.vue";
import Rights from "@/components/Rights/Index.vue";
import RightsHistory from "@/components/Rights/RightsHistory.vue";
import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";
import Bill from "@/components/Claim/Bill.vue";
import Bookbank from "@/components/Claim/Bookbank.vue";
import Preview from "@/components/Claim/Preview.vue";
import ConfirmOTP from "@/components/Claim/ConfirmOTP.vue";
import CheckStatus from "@/components/Status/CheckStatus.vue";
import ClaimDetail from "@/components/Status/ClaimDetail.vue";
import ConfirmMoney from "@/components/Status/ConfirmMoney.vue";
const routes = [
    // {
    //     path: "/",
    //     name: "Home",
    //     component: Home,
    // },
    {
        path: "/Advice",
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
        path: "/Counter",
        name: "Counter",
        component: Counter,
    },
    {
        path: "/FetchData",
        name: "FetchData",
        component: FetchData,
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
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;