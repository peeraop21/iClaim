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
            <AccidentCarInfo ref="accidentCarInfo" :hasSubmit="hasSubmit" :provinces="init.provinces" :cars="init.cars"></AccidentCarInfo>
            <AccidentInfo class="mt-3" ref="accidentInfo" :hasSubmit="hasSubmit" :provinces="init.provinces"></AccidentInfo>
            <AccidentVictimInfo class="mt-3" ref="accidentVictimInfo" :hasSubmit="hasSubmit" :provinces="init.provinces" ></AccidentVictimInfo>
            <button class="btn-next-submit mt-5 mb-3" style="width: 100%; padding: 8px 0px;" type="submit">ยืนยัน</button>
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
        name: 'AccidentEdit',
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
                    cars: null,
                    currentAddress:null
                },
                isLoading: true,
                formIsValid:false,
                hasSubmit:false,
            }
        },
        computed: {
        },
        methods: {
            goToConfirmOTP() {
                this.$router.push({ name: 'ConfirmOTP', params: { id:'EditAccident', from: 'AccidentEdit' } })
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

                if (this.$refs.createAccident.checkValidity() && this.$refs.accidentCarInfo.input.accCarPolicyNo && this.$refs.accidentInfo.input.accImages.length > 0 && this.$refs.accidentCarInfo.input.accCarImages.length > 0 && this.$refs.accidentVictimInfo.input.accVicBrokenImages.length > 0) {
                    this.formIsValid = true
                    this.goToConfirmOTP();
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
                        console.log(response)
                        this.init.provinces = response.data.provinces;
                        this.init.cars = response.data.cars;
                        this.init.currentAddress = response.data.currentAddress;

                        if (this.init.currentAddress) {
                            console.log("come: ",this.init.currentAddress)
                            this.$refs.accidentVictimInfo.input.accVicCurrentHomeId = this.init.currentAddress.houseNo
                            this.$refs.accidentVictimInfo.input.accVicCurrentMoo = this.init.currentAddress.moo
                            this.$refs.accidentVictimInfo.input.accVicCurrentSoi = this.init.currentAddress.soi
                            this.$refs.accidentVictimInfo.input.accVicCurrentRoad = this.init.currentAddress.road
                            this.$refs.accidentVictimInfo.input.accVicCurrentProv = this.init.currentAddress.province
                            this.$refs.accidentVictimInfo.onChange("changwat").then((res) => {
                                if (res === "reqProvinceSuccess") {
                                    this.$refs.accidentVictimInfo.input.accVicCurrentDist = this.init.currentAddress.district
                                    this.$refs.accidentVictimInfo.onChange("amphur").then((res) => {
                                        if (res === "reqDistrictSuccess") {
                                            this.$refs.accidentVictimInfo.input.accVicCurrentSubDist = this.init.currentAddress.subDistrict
                                        }
                                    }).catch((err) => {
                                        this.isLoading = false
                                        alert(err);
                                    })
                                }
                            }).catch((err) => {
                                this.isLoading = false
                                alert(err);
                            })
                        }
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
    #accidentFile .filepond--list-scroller .filepond--list .filepond--item {
        width: calc(50% - 0.5em);
    }
        #accidentFile .filepond--list-scroller .filepond--list .filepond--item .filepond--file-wrapper .filepond--file .filepond--file-status {
            width: calc(50% - 0.5em);
        }
    .un-input-image {
        width: 100%;
        margin-top: 0.25rem;
        font-size: 0.875em;
        color: #dc3545;
    }
    
</style>
