<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-title">ข้อมูลรายละเอียดอุบัติเหตุ</div>
            <div class="row">
                <div class="col-8">
                    <label for="accDateInput" class="form-label">วันที่เกิดเหตุ<span class="star-require">*</span></label>
                    <v-date-picker v-model="input.accDate" class="flex-grow " locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                        <template v-slot="{ inputValue, inputEvents }">
                            <input class=" mt-0 mb-2 form-control"
                                   :value="inputValue"
                                   id="accDateInput"
                                   v-on="inputEvents"
                                   @keydown="clearInput"
                                   required />
                            <div class="invalid-feedback">
                                กรุณาเลือกวันที่เกิดเหตุ.
                            </div>
                        </template>
                    </v-date-picker>
                </div>
                <div class="col-4">
                    <label for="accTimeInput" class="form-label">เวลา<span class="star-require">*</span></label>
                    <v-date-picker v-model="input.accTime" class="flex-grow " locale="th" mode="time" is24hr :model-config="timeModelConfig" :dayPopover="{}">
                        <template v-slot="{ inputValue, inputEvents }">
                            <input class=" mt-0 mb-2 form-control "
                                   :value="inputValue"
                                   id="accTimeInput"
                                   v-on="inputEvents"
                                   @keydown="clearInput"
                                   required />
                            <div class="invalid-feedback">
                                กรุณาเลือกเวลาที่เกิดเหตุ.
                            </div>
                        </template>
                    </v-date-picker>

                </div>
            </div>
            <div class="row">
                <div class="col-6 form-group">
                    <label for="accProvInput" class="form-label">จังหวัดที่เกิดเหตุ<span class="star-require">*</span></label>
                    <select v-model="input.accProv" class="form-select mt-0 mb-2 form-control" id="accProvInput" @change="onChange('changwat')" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option v-for="(item, index) in provinces" :key="index" :value="item.changwatshortname" >{{item.changwatname}}</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกจังหวัด.
                    </div>
                </div>
                <div class="col-6">
                    <label for="accDistInput" class="form-label">อำเภอที่เกิดเหตุ<span class="star-require">*</span></label>
                    <select v-model="input.accDist" class="form-select mt-0 mb-2 form-control" :disabled="!input.accProv" id="accDistInput" @change="onChange('amphur')" required>
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
                    <label for="accSubDistInput" class="form-label">ตำบลที่เกิดเหตุ<span class="star-require">*</span></label>
                    <select v-model="input.accSubDist" class="form-select mt-0 mb-2 form-control" :disabled="!input.accDist" id="accSubDistInput" required>
                        <option :value="null" disabled selected>เลือก...</option>
                        <option v-for="(item, index) in init.subDistricts" :key="index" :value="item.tumbolid">{{item.tumbolname}}</option>
                    </select>
                    <div class="invalid-feedback">
                        กรุณาเลือกตำบล.
                    </div>
                </div>
                <div class="col-6">
                    <label for="accPlaceInput" class="form-label">สถานที่เกิดเหตุ<span class="star-require">*</span></label>
                    <b-form-input v-model="input.accPlace" class="mt-0 mb-2" id="accPlaceInput" type="text" required></b-form-input>
                    <div class="invalid-feedback">
                        กรุณากรอกสถานที่เกิดเหตุ.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <label for="accDetailInput" class="form-label">ลักษณะการเกิดเหตุ<span class="star-require">*</span></label>
                    <textarea v-model="input.accDetail" class="form-control mt-0 mb-2" id="accDetailInput" placeholder="เช่น ขี่รถตกหลุมรถพลิกคว่ำ" required></textarea>
                    <div class="invalid-feedback">
                        กรุณากรอกลักษณะการเกิดเหตุ.
                    </div>
                </div>
                
            </div>
            <div class="row">
                <div class="col-12">
                    <label for="accImagesInput" class="form-label">รูปภาพสถานที่เกิดเหตุ<span class="star-require">*</span></label>
                    <InputImg ref="accImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปสถานที่เกิดเหตุ.
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
        name: 'AccidentInfo',
        props: ['provinces', 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
                attrs: [
                    {
                        key: 'today',
                        dot: true,
                        dates: new Date(),
                    },
                ],
                dateModelConfig: {
                    type: 'string',
                    mask: 'YYYY-MM-DD',
                },
                timeModelConfig: {
                    type: 'string',
                    mask: 'HH:mm',
                },
                date: new Date(),
                init: {
                    districts: null,
                    subDistricts:null
                },
                input: {
                    accDate: null,
                    accTime: null,
                    accProv: null,
                    accDist: null,
                    accSubDist: null,
                    accPlace: null,
                    accDetail: null,
                    accBranchId: null,
                    accImages:[]
                }
            }
        },

        methods: {
            storeFile(file) {
                console.log("file come: ", file)
                this.input.accImages = file
            },
            clearInput(event) {
                event.preventDefault();
            },
            onChange(elementName) {
                var url = this.$store.state.envUrl
                if (elementName == 'changwat') {
                    this.input.accBranchId = this.provinces.filter(w => w.changwatshortname === this.input.accProv).map(s => s.branchid)[0]
                    url = url + '/api/Master/Amphurs';
                    axios.get(url, { params: { changwatshortname: this.input.accProv } })
                        .then((response) => {
                            this.init.districts = response.data;
                        })
                        .catch(function (error) {
                            alert(error);
                        });
                    this.input.accDist = null
                    this.input.accSubDist = null
                } else if (elementName == 'amphur') {
                    url = url + '/api/Master/Tumbols';
                    axios.get(url, { params: { changwatshortname: this.input.accProv, amphurId: this.input.accDist } })
                        .then((response) => {
                            this.init.subDistricts = response.data;
                        })
                        .catch(function (error) {
                            alert(error);
                        });
                    this.input.accSubDist = null
                } else if (elementName == 'tumbol') {
                    console.log("tumbol")
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
    .accident-title {
        text-align: center;
        font-size: 16px;
        text-decoration: underline;
        margin-bottom: 10px;
    }
    .form-label {
        margin-bottom: 0px;
    }
    textarea{
        height:5rem;
    }
</style>
