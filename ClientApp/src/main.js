import Vue from 'vue'
import Vuelidate from 'vuelidate'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import dateFilter from './filters/date.filter'
import messagePlugin from '@/utils/message.plugin'
import Loader from '@/components/app/Loader'
import store from './store'
import 'materialize-css/dist/js/materialize.min'
import currencyFilter from './filters/currency.filter'



Vue.config.productionTip = false

Vue.filter('currency', currencyFilter)
Vue.filter('date', dateFilter)
Vue.use(Vuelidate)
Vue.use(messagePlugin)
Vue.component('Loader', Loader)

let app = new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')




