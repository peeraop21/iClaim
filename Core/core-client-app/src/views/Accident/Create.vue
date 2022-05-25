<template>
    <div class="container">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">
        </loading>
        <h2 id="header2">CreateAccident</h2>
        <form ref="createAccident" class="row mx-0 needs-validation" @submit="onSubmit" novalidate>
            <AccidentInfo :provinces="init.provinces"></AccidentInfo>
            <AccidentCarInfo ref="accidentCarInfo" :hasSubmit="hasSubmit" :provinces="init.provinces" :cars="init.cars" class="mt-3"></AccidentCarInfo>
            <button class="btn-next-submit mt-5" style="width: 100%; padding: 8px 0px;" type="submit">ยืนยัน</button>
        </form>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import AccidentInfo from '../../components/AccidentCreate/AccidentInfo.vue'
    import AccidentCarInfo from '../../components/AccidentCreate/AccidentCarInfo.vue'
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
            AccidentCarInfo
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
            onSubmit() {
                var validateGroup = document.getElementsByClassName('validate-me')
                console.log(validateGroup)
                event.preventDefault();
                for (let i = 0; i < validateGroup.length; i++) {
                    validateGroup[i].classList.add('was-validated');
                }
                this.hasSubmit = true

                if (this.$refs.createAccident.checkValidity() && this.$refs.accidentCarInfo.input.accCarPolicyNo) {
                    this.formIsValid = true
                } else {
                    this.formIsValid = false
                }
                    
                console.log(this.formIsValid)
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
