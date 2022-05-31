<template>
    <div class="container">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">
        </loading>
        <h2 id="header2">ข้อมูลอุบัติเหตุ</h2>
        <div class="row px-3">
            <p align="left">&emsp;กรอกข้อมูลอุบัติเหตุส่งให้เจ้าหน้าที่ตรวจสอบ เพื่อใช้สิทธิ์เบิกค่ารักษาพยาบาล</p>
        </div>
        <div class="row">
            <AccidentEditInfo></AccidentEditInfo>
            <AccidentCarEditInfo></AccidentCarEditInfo>
            <AccidentVictimEditInfo></AccidentVictimEditInfo>
        </div>

    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'

    import axios from 'axios'

    import AccidentEditInfo from '../../components/AccidentEdit/AccidentEditInfo.vue'
    import AccidentCarEditInfo from '../../components/AccidentEdit/AccidentCarEditInfo.vue'
    import AccidentVictimEditInfo from '../../components/AccidentEdit/AccidentVictimEditInfo.vue'

    // Import component
    import Loading from 'vue-loading-overlay';
    // Import stylesheet
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'AccidentEdit',
        components: {
            Loading,
            AccidentEditInfo,
            AccidentCarEditInfo,
            AccidentVictimEditInfo
        },
        mixins: [mixin],
        data() {
            return {

            }
        },
        computed: {
        },
        methods: {
            goToConfirmOTP() {
                this.$router.push({ name: 'ConfirmOTP', params: { id: 'EditAccident', from: 'AccidentEdit' } })
            },
            getDataForThisPage() {
                var url = this.$store.state.envUrl + '/api/accident/DataForAccidentEditPage';
                const body = {
                    AccNo: this.$route.params.accNo,
                    UserIdCard: this.$store.state.userStateData.idcardNo,
                };
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(body), apiConfig)
                    .then((response) => {
                        console.log(response)
                    }).catch((error) => {
                        alert(error)
                    })

            }
        },
        created() {
            this.getDataForThisPage()
        },

    }
</script>

<style>
   
</style>
