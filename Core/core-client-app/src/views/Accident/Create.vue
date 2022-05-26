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
        <form ref="createAccident" class="row mx-0 needs-validation" @submit="onSubmit" novalidate>
            <AccidentInfo ref="accidentInfo" :provinces="init.provinces"></AccidentInfo>
            <AccidentCarInfo class="mt-3" ref="accidentCarInfo" :hasSubmit="hasSubmit" :provinces="init.provinces" :cars="init.cars"></AccidentCarInfo>
            <AccidentVictimInfo ref="accidentVictimInfo" class="mt-3" :provinces="init.provinces" ></AccidentVictimInfo>
            <button class="btn-next-submit mt-5" style="width: 100%; padding: 8px 0px;" type="submit">ยืนยัน</button>
        </form>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import AccidentInfo from '../../components/AccidentCreate/AccidentInfo.vue'
    import AccidentCarInfo from '../../components/AccidentCreate/AccidentCarInfo.vue'
    import AccidentVictimInfo from '../../components/AccidentCreate/AccidentVictimInfo.vue'
    import axios from 'axios'
    // Import component
    import Loading from 'vue-loading-overlay';
    // Import stylesheet
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'AccidentCreate',
        components: {
            Loading,
            AccidentInfo,
            AccidentCarInfo,
            AccidentVictimInfo
        },
        mixins: [mixin],
        data() {
            return {
                init: {
                    provinces: null,
                    cars:null
                },
                isLoading: true,
                formIsValid:false,
                hasSubmit:false,
            }
        },
        computed: {
        },
        methods: {
            postDataTest() {
                var url = this.$store.state.envUrl + '/api/accident/InsertAccident';
                const ReqPostAccident = this.$store.state.inputAccidentData;
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(ReqPostAccident), apiConfig)
                    .then((response) => {
                        console.log(response)
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            storeData() {
                this.$store.state.inputAccidentData.AccidentInput = this.$refs.accidentInfo.input
                this.$store.state.inputAccidentData.AccidentCarInput = this.$refs.accidentCarInfo.input
                this.$store.state.inputAccidentData.AccidentVictimInput = this.$refs.accidentVictimInfo.input
            },
            onSubmit() {
                var validateGroup = document.getElementsByClassName('validate-me')
                event.preventDefault();
                for (let i = 0; i < validateGroup.length; i++) {
                    validateGroup[i].classList.add('was-validated');
                }
                this.hasSubmit = true
                this.storeData();
                
                if (this.$refs.createAccident.checkValidity() && this.$refs.accidentCarInfo.input.accCarPolicyNo) {
                    this.formIsValid = true
                    this.postDataTest();
                    console.log(this.formIsValid, this.$store.state.inputAccidentData)
                } else {
                    this.formIsValid = false
                    console.log(this.formIsValid, this.$store.state.inputAccidentData)
                }
                                               
            },
            getDataForThisPage() {
                var url = this.$store.state.envUrl + '/api/accident/DataForAccidentCreatePage';
                const body = {
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
                        this.init.provinces = response.data.provinces;
                        this.init.cars = response.data.cars;
                        this.isLoading = false
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        created() {
            this.getDataForThisPage()
        },

    }
</script>

<style>
    
</style>
