import { createWebHistory, createRouter } from "vue-router";
//import Home from "@/components/Home/Home.vue";
import Counter from "@/components/Counter.vue";
import FetchData from "@/components/FetchData.vue";
import Advice from "@/components/Advice/Index.vue";
import Accident from "@/components/Accident/Index.vue";
import Rights from "@/components/Rights/Index.vue";
import RightsHistory from "@/components/Rights/RightsHistory.vue";
import Bill from "@/components/Claim/Bill.vue";
import Bookbank from "@/components/Claim/Bookbank.vue";
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
        path: "/Counter",
        name: "Counter",
        component: Counter,
    },
    {
        path: "/FetchData",
        name: "FetchData",
        component: FetchData,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;