import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import { BootstrapVue, IconsPlugin, CalendarPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueFormWizard from 'vue-form-wizard'
import 'vue-form-wizard/dist/vue-form-wizard.min.css'
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import VCalendar from 'v-calendar';
import Vuesax from 'vuesax'
import 'vuesax/dist/vuesax.css'
import Vuelidate from 'vuelidate'
import OtpInput from "@bachdgvn/vue-otp-input";
import vSelect from 'vue-select'
import 'vue-select/dist/vue-select.css';
import "vue-select/src/scss/vue-select.scss";



Vue.use(Vuesax, {
  colors: {
    primary:'#5b3cc4',
    success:'rgb(23, 201, 100)',
    danger:'rgb(242, 19, 93)',
    warning:'rgb(255, 130, 0)',
    dark: 'rgb(36, 33, 69)',
    light: "#fff"
  }
})


Vue.component("v-otp-input", OtpInput);
Vue.component('v-select', vSelect)



import StarRating from 'vue-star-rating'
Vue.use(StarRating)

Vue.config.ignoredElements = [/^ion-/]
Vue.use(CalendarPlugin)
Vue.use(VCalendar)
Vue.use(VueSweetalert2)
Vue.use(VueFormWizard)
Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(Vuelidate)
Vue.config.productionTip = false
/*Vue.prototype.$testOverlay = false*/

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')


