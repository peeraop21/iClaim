<template>
    <div>
        <div class="box-container shadow-box px-2">
            <div class="accident-car-title">ข้อมูลรถที่เกิดอุบัติเหตุ</div>
            <label class="form-label my-1">
                เลือกรถที่เกิดเหตุ<span class="star-require">*</span>
            </label>
            <section>
                <div style="height: 98%; width: 100%;">
                    <div v-for="(item, index) in cars" :key="index">
                        <div class="accordion">
                            <div class="accordion-item" :id="item.policyNo">
                                <a class="accordion-link" @click="onClickCar(item.policyNo)">
                                    <div class="radio-select-car" style="float: left">
                                        <vs-radio class="white-vs-radio" v-model="input.accCarPolicyNo" :val="item.policyNo" :id="item.policyNo">
                                            คันที่ {{index + 1}} ทะเบียน : {{item.carLicense}} {{provinces.filter(w => w.provinceId === item.carProvince).map(s => s.changwatname).toString()}}
                                        </vs-radio>
                                        <!--<input v-model="input.accCarPolicyNo" :value="item.policyNo" class="form-check-input" type="radio" name="flexRadioDefault" :id="item.policyNo">
                                        <label class="form-check-label" :for="item.policyNo">
                                            คันที่ {{index + 1}} ทะเบียน : {{item.carLicense}} {{provinces.filter(w => w.provinceId === item.carProvince).map(s => s.changwatname).toString()}}
                                        </label>-->
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer" :class="{ 'car-detail': input.accCarPolicyNo == item.policyNo}">
                                    <div class="row d-inline">
                                        <label class="form-label">เลขกรมธรรม์ : {{item.policyNo}}</label>
                                    </div>
                                    <div class="row d-inline">
                                        <label class="form-label">รุ่น : {{item.marque}}</label>
                                    </div>
                                    <div class="row d-inline">
                                        <label class="form-label">ขนาด : {{item.engineSize}} cc.</label>
                                    </div>
                                    <div class="row d-inline">
                                        <label class="form-label">เลขตัวถัง : {{item.carTankNo}}</label>
                                    </div>
                                </div>
                                <div style="text-align: center">

                                </div>
                            </div>
                        </div>
                    </div>

                    <!--<div>
                        <div class="accordion">
                            <div class="accordion-item" :id="'New'">
                                <a class="accordion-link" @click="onClickCar('New')">
                                    <div class="radio-select-car">
                                        <vs-radio class="white-vs-radio" v-model="input.accCarPolicyNo" :val="'New'" :id="'New'">
                                            อื่นๆ
                                        </vs-radio>-->
                                        <!--<input v-model="input.accCarPolicyNo" :value="'New'" class="form-check-input" type="radio" name="flexRadioDefault" :id="'New'">
                                        <label class="form-check-label" :for="'New'">
                                            อื่นๆ
                                        </label>-->
                                    <!--</div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer " :class="{ 'new': input.accCarPolicyNo == 'New'}">
                                    <div class="d-inline">
                                        <div class="form-check red-car-license-checkbox">
                                            <input v-model="input.accCarIsRedCarLicense" class="form-check-input" type="checkbox" value="true" id="isRedCarLicense">
                                            <label class="form-check-label" for="isRedCarLicense">
                                                ป้ายแดง
                                            </label>
                                        </div>
                                    </div>

                                    <div v-if="!input.accCarIsRedCarLicense" class="d-inline" :class="{ 'validate-me': input.accCarPolicyNo == 'New'}">
                                        <label for="carLicenseInput" class="form-label">ทะเบียนรถที่เกิดเหตุ<span class="star-require">*</span></label>
                                        <label for="carLicenseProvInput" class="form-label pe-2 float-end">จังหวัดที่เกิดเหตุ<span class="star-require">*</span></label>
                                        <div class="row">
                                            <div class="col pe-0">
                                                <b-form-input @input="onInputCarLicense" class="mt-0 mb-2" id="startCarLicenseInput" type="text" placeholder="1กข" :disabled="input.accCarPolicyNo != 'New'" :required="input.accCarPolicyNo == 'New'"></b-form-input>
                                                <div class="invalid-feedback">
                                                    กรุณากรอกทะเบียนรถที่เกิดเหตุ.
                                                </div>
                                            </div>
                                            <div class="col px-0">
                                                <b-form-input @input="onInputCarLicense" class="mt-0 mb-2" id="endCarLicenseInput" type="number" placeholder="1234" :disabled="input.accCarPolicyNo != 'New'" :required="input.accCarPolicyNo == 'New'"></b-form-input>
                                            </div>
                                            <div class="col-5">
                                                <select v-model="input.accCarLicenseProv" class="form-select mt-0 mb-2 form-control" id="carLicenseProvInput" :disabled="input.accCarPolicyNo != 'New'" :required="input.accCarPolicyNo == 'New'">
                                                    <option :value="null" disabled selected>จังหวัด...</option>
                                                    <option v-for="(item, index) in provinces" :key="index" :value="item.changwatshortname">{{item.changwatname}}</option>
                                                </select>
                                                <div class="invalid-feedback">
                                                    กรุณาเลือกจังหวัด.
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="d-inline" :class="{ 'validate-me': input.accCarPolicyNo == 'New' &&  input.accCarIsRedCarLicense }">
                                        <label for="carTankNoInput" class="form-label">เลขตัวถังรถที่เกิดเหตุ<span v-if="input.accCarIsRedCarLicense" class="star-require">*</span></label>
                                        <b-form-input v-model="input.accCarTankNo" class="mt-0 mb-2" id="carTankNoInput" type="text" :disabled="input.accCarPolicyNo != 'New'" :required="input.accCarPolicyNo == 'New' &&  input.accCarIsRedCarLicense"></b-form-input>
                                        <div class="invalid-feedback">
                                            กรุณากรอกเลขตัวถังรถที่เกิดเหตุ.
                                        </div>
                                    </div>
                                </div>
                                <div style="text-align: center">

                                </div>
                            </div>
                        </div>
                    </div>-->
                    <div v-if="!input.accCarPolicyNo && hasSubmit" class="un-select-car">
                        กรุณาเลือกรถที่เกิดเหตุ.
                    </div>
                </div>
            </section>
            <div class="row">
                <div class="col-12">
                    <label for="accCarImagesInput" class="form-label">รูปภาพรถที่เกิดเหตุ (รถที่ประสบอุบัติเหตุ)<span class="star-require">*</span></label>
                    <InputImg ref="accCarImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accCarImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปรถที่เกิดเหตุ.
                    </div>
                </div>
                
            </div>

        </div>
    </div>

</template>

<script>

    //import axios from 'axios'
    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentCarInfo',
        props: ['provinces', 'cars', 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
                
                input: {
                    accCarPolicyNo: null,
                    accCarLicense: null,
                    accCarLicenseProv: null,
                    accCarTankNo: null,
                    accCarEngineSize:null,
                    accCarTypeCarId:null,
                    accCarIsRedCarLicense: false,
                    accCarDrivePrefixName: this.$store.state.userStateData.prefix,
                    accCarDriveFirstname: this.$store.state.userStateData.fname,
                    accCarDriveLastname: this.$store.state.userStateData.lname,
                    accCarDriveTelNo: this.$store.state.userStateData.mobileNo,
                    accCarDriveProtectStartDate: null,
                    accCarDriveProtectStartTime: null,
                    accCarDriveProtectEndDate:null,
                    accCarDriveProtectEndTime: null,
                    accCarImages:[]
                },
            }
        },
        computed: {

        },
        methods: {
            storeFile(file) {
                
                this.input.accCarImages = file
                console.log("file car come: ", this.input.accCarImages)
            },
            onInputCarLicense() {
                this.input.accCarLicense = document.getElementById("startCarLicenseInput").value + '-' + document.getElementById("endCarLicenseInput").value
            },
            onClickCar(value) {
                if (value != 'New') {
                    window.location.href = '#' + value
                    document.getElementById(value).checked = true
                    this.input.accCarPolicyNo = value
                    this.input.accCarLicense = this.cars.filter(w => w.policyNo === value).map(s => s.carLicense).toString()
                    this.input.accCarLicenseProv = this.provinces.filter(w => w.provinceId === this.cars.filter(w => w.policyNo === value).map(s => s.carProvince).toString()).map(s => s.changwatshortname).toString()
                    this.input.accCarTankNo = this.cars.filter(w => w.policyNo === value).map(s => s.carTankNo).toString()
                    this.input.accCarEngineSize = this.cars.filter(w => w.policyNo === value).map(s => s.engineSize).toString()
                    this.input.accCarTypeCarId = this.cars.filter(w => w.policyNo === value).map(s => s.carTypeId).toString()
                    this.input.accCarDriveProtectStartDate = this.cars.filter(w => w.policyNo === value).map(s => s.startDate).toString()
                    this.input.accCarDriveProtectStartTime = this.cars.filter(w => w.policyNo === value).map(s => s.startTime).toString()
                    this.input.accCarDriveProtectEndDate = this.cars.filter(w => w.policyNo === value).map(s => s.endDate).toString()
                    this.input.accCarDriveProtectEndTime = this.cars.filter(w => w.policyNo === value).map(s => s.endTime).toString()
                    console.log("input: ", this.input)
                } else {
                    window.location.href = '#' + value
                    document.getElementById(value).checked = true
                    this.input.accCarPolicyNo = value
                    this.input.accCarLicense = null
                    this.input.accCarLicenseProv = null
                    this.input.accCarTankNo = null
                    console.log("input: ",this.input)
                }
                
            }

        },

        mounted() {


        },
        created() {

        },

    }
</script>

<style scoped>
    
    .radio-select-car {
        margin-bottom: 5px;
        float: left;
    }
    .accident-car-title {
        text-align: center;
        font-size: 16px;
        text-decoration: underline;
        margin-bottom: 10px;
    }

    .red-car-license-checkbox {
        padding-left: 1.5rem !important;
    }

    input[type=radio] {
        transform: scale(1.5);
    }

    input[type=checkbox] {
        transform: scale(1.5);
    }
    .un-select-car {
        width: 100%;
        margin-top: 0.25rem;
        font-size: 0.875em;
        color: #dc3545;
    }

    .form-label {
        margin-bottom: 0px;
    }

    .accordion-item {
        border: none;
        border-radius: 2rem !important;
        box-shadow: 3px 3px 5px -4px #888888;
        background-color: #e1deec;
    }
    .accordion-link {
        background-color: #e1deec;
    }

    .answer {
        background-color:#fff;
        border-radius: 1rem !important;
        padding: 0px 5px;
    }

        .answer::before {
            display: none;
        }
    .car-detail {
        padding: 10px 10px !important;
    }
    .new {
        padding: 10px 5px !important;
    }
</style>
