import { createWebHistory, createRouter } from "vue-router";
//import Home from "@/components/Home/Home.vue";
import Counter from "@/components/Counter.vue";
import FetchData from "@/components/FetchData.vue";
import Introduce from "@/components/Introduce.vue";
import AccidentList from "@/components/AccidentList.vue";
import RightsReceive from "@/components/RightsReceive.vue";
import Boto from "@/components/Boto.vue";
import Bill from "@/components/Bill.vue";
import Bookbank from "@/components/Bookbank.vue";
const routes = [
    // {
    //     path: "/",
    //     name: "Home",
    //     component: Home,
    // },
    {
        path: "/",
        name: "Introduce",
        component: Introduce,
    },
    {
        path: "/AccidentList",
        name: "AccidentList",
        component: AccidentList,
    },
    {
        path: "/RightsReceive",
        name: "RightsReceive",
        component: RightsReceive,
    },
    {
        path: "/Boto",
        name: "Boto",
        component: Boto,
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