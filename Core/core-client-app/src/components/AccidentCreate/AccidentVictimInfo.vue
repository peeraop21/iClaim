<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-victim-title fw-bold">ข้อมูลผู้ประสบภัย</div>
            <div class="row">
                <div class="col-8">
                    <label for="accVicCurrentHomeId" class="form-label">บ้านเลขที่<span class="star-require">*</span></label>
                    <b-form-input v-model="input.accVicCurrentHomeId" class="mt-0 mb-2" id="accVicCurrentHomeId" type="text" required></b-form-input>
                    <div class="invalid-feedback">
                        กรุณากรอกบ้านเลขที่.
                    </div>
                </div>
                <div class="col-4">
                    <label for="accVicCurrentMoo" class="form-label">หมู่ที่<span class="star-require">*</span></label>
                    <b-form-input v-model="input.accVicCurrentMoo" class="mt-0 mb-2" id="accVicCurrentMoo" type="text" required></b-form-input>
                    <div class="invalid-feedback">
                        กรุณากรอกหมู่ที่.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <label for="accVicCurrentSoi" class="form-label">ซอย<span class="star-require">*</span></label>
                    <b-form-input v-model="input.accVicCurrentSoi" class="mt-0 mb-2" id="accVicCurrentSoi" type="text" required></b-form-input>
                    <div class="invalid-feedback">
                        กรุณากรอกซอย.
                    </div>
                </div>
                <div class="col-6">
                    <label for="accVicCurrentRoad" class="form-label">ถนน<span class="star-require">*</span></label>
                    <b-form-input v-model="input.accVicCurrentRoad" class="mt-0 mb-2" id="accVicCurrentRoad" type="text" required></b-form-input>
                    <div class="invalid-feedback">
                        กรุณากรอกถนน.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-6 form-group">
                    <label for="accVicCurrentProv" class="form-label">จังหวัด<span class="star-require">*</span></label>
                    <select v-model="input.accVicCurrentProv" class="form-select mt-0 mb-2 form-control" id="accVicCurrentProv" @change="onChange('changwat')" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option v-for="(item, index) in provinces" :key="index" :value="item.changwatshortname">{{item.changwatname}}</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกจังหวัด.
                    </div>
                </div>
                <div class="col-6">
                    <label for="accVicCurrentDist" class="form-label">อำเภอ<span class="star-require">*</span></label>
                    <select v-model="input.accVicCurrentDist" class="form-select mt-0 mb-2 form-control" :disabled="!input.accVicCurrentProv" id="accVicCurrentDist" @change="onChange('amphur')" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option v-for="(item, index) in init.districts" :key="index" :value="item.amphurid">{{item.amphurname}}</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกอำเภอ.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <label for="accVicCurrentSubDist" class="form-label">ตำบล<span class="star-require">*</span></label>
                    <select v-model="input.accVicCurrentSubDist" class="form-select mt-0 mb-2 form-control" :disabled="!input.accVicCurrentDist" id="accVicCurrentSubDist" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option v-for="(item, index) in init.subDistricts" :key="index" :value="item.tumbolid">{{item.tumbolname}}</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกตำบล.
                    </div>
                </div>
                <div class="col-6">
                    <label for="accVicCaseType" class="form-label">ประเภทผู้ป่วย<span class="star-require">*</span></label>
                    <select v-model="input.accVicCaseType" class="form-select mt-0 mb-2 form-control" id="accVicCaseType" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option :value="'OPD'">ผู้ป่วยนอก</option>
                        <option :value="'IPD'">ผู้ป่วยใน</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกประเภทผู้ป่วย.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <label for="accVicBrokenDetail" class="form-label">ลักษณะอาการบาดเจ็บ<span class="star-require">*</span></label>
                    <textarea v-model="input.accVicBrokenDetail" class="form-control mt-0 mb-2" id="accVicBrokenDetail" placeholder="เช่น แผลถลอก, แขน/ขาถลอก, มีแผลตามร่างกาย, แผลเย็บ, หัวแตก" required></textarea>
                    <div class="invalid-feedback">
                        กรุณากรอกลักษณะอาการบาดเจ็บ.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <label for="accVicBrokenImagesInput" class="form-label">รูปภาพอาการบาดเจ็บ<span class="star-require">*</span></label>
                    <InputImg ref="accVicBrokenImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accVicBrokenImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปอาการบาดเจ็บ.
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>

    import axios from 'axios'
    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentVictimInfo',
        props: ['provinces', 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
                init: {
                    districts: null,
                    subDistricts: null
                },
                input: {
                    accVicCurrentHomeId: null,
                    accVicCurrentMoo: null,
                    accVicCurrentSoi: null,
                    accVicCurrentRoad: null,
                    accVicCurrentProv: null,
                    accVicCurrentDist: null,
                    accVicCurrentSubDist: null,
                    accVicCaseType: null,
                    accVicBrokenDetail:null,
                    accVicUserLineId: this.$store.state.userTokenLine,
                    accVicIdCardNo: this.$store.state.userStateData.idcardNo,
                    accVicPrefixName: this.$store.state.userStateData.prefix,
                    accVicFirstname: this.$store.state.userStateData.fname,
                    accVicLastname: this.$store.state.userStateData.lname,
                    accVicTelNo: this.$store.state.userStateData.mobileNo,
                    accVicDateOfBirth: this.$store.state.userStateData.stringDateofBirth,
                    accVicBrokenImages:[]

                },
            }
        },
        computed: {

        },
        methods: {
            storeFile(file) {
                this.input.accVicBrokenImages = file
            },
            async onChange(elementName) {
                var url = this.$store.state.envUrl
                if (elementName == 'changwat') {
                    url = url + '/api/Master/Amphurs';
                    await axios.get(url, { params: { changwatshortname: this.input.accVicCurrentProv } })
                        .then((response) => {
                            this.init.districts = response.data;
                        })
                        .catch(function (error) {
                            alert(error);
                        });
                    this.input.accVicCurrentDist = null
                    this.input.accVicCurrentSubDist = null
                    return "reqProvinceSuccess"
                } else if (elementName == 'amphur') {
                    url = url + '/api/Master/Tumbols';
                    await axios.get(url, { params: { changwatshortname: this.input.accVicCurrentProv, amphurId: this.input.accVicCurrentDist } })
                        .then((response) => {
                            this.init.subDistricts = response.data;
                        })
                        .catch(function (error) {
                            alert(error);
                        });
                    this.input.accVicCurrentSubDist = null
                    return "reqDistrictSuccess"
                } else if (elementName == 'tumbol') {
                    console.log("tumbol")
                    return
                }
            }

        },

        mounted() {


        },
        created() {
            console.log(this.$store.state.userStateData)
        },

    }
</script>

<style scoped>
    .accident-victim-title {
        text-align: center;
        font-size: 16px;
        text-decoration: underline;
        margin-bottom: 10px;
    }
    .form-label {
        margin-bottom: 0px;
    }
    textarea {
        height: 5rem;
    }
</style>
