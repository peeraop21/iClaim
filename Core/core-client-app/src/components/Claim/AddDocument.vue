<template>
    <div class="container" align="center">
        <h2 id="header2" class="mb-4">กรอกข้อมูล/แนบเอกสารเพิ่มเติม</h2>
        <div align="left">
            <p style="font-weight: bold">
                <ion-icon name="newspaper-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> เลขที่รับแจ้ง:
                <span class="p-main-color">{{ hosData.accNo }}</span> คำร้องที่:
                <span class="p-main-color">{{ hosData.appNo }}</span>
                <br>
                <ion-icon name="calendar-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> วันที่ยื่นคำร้อง:
                <span class="p-main-color">{{ hosData.stringRegDate }}</span>
                <br>
            </p>
        </div>
        <div class="tab-user mb-3">
            <div class="row">
                <div class="col-3 px-0">
                    <img src="@/assets/kid.png" width="70">
                </div>
                <div class="col-9 text-start px-2">
                    <span>ชื่อ-สกุล: <span style="color: var(--main-color)">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</span></span>
                    <br>
                    <span>เบอร์โทรศัพท์: <span style="color: var(--main-color)">{{userData.mobileNo}}</span></span>
                    <br>
                    <span>สถานะ: <span style="color: var(--main-color)">ผู้ประสบภัยจากรถ</span></span>
                </div>
            </div>
        </div>
        <div align="left" class="mb-3">
            <p>
                มีความจำนงค์ในการติดต่อ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด เพื่อกระทำการ
                <span class="p-main-color"> ยื่นคำร้องขอรับเงินเบิกค่าเสียหายเบื้องต้น</span>
            </p>
            <p class="p-main-color">จำนวนเงิน {{hosData.sumMoney}} บาท</p>
            <p>
                จากกรมธรรม์ประกันภัยของบริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด กรณีการเกิดอุบัติเหตุของ
                <br />
                รถหมายเลขทะเบียน
                <span v-for="(car, index) in accData.car" :key="`car-${index}`" class="p-main-color">{{car}} </span>
                <br />
                ในวันที่
                <span class="p-main-color"> {{accData.stringAccDate}}</span>
            </p>
            <p style="margin-bottom: 0px">โดยมีเอกสารหลักฐานที่นำมาแสดง ประกอบด้วย</p>
            <vs-checkbox color="var(--main-color)" v-model="invoiceDoc" style="margin-top: -15px" disabled>
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                <p style="margin-top: 15px; font-size: 13px">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="accountDoc" style="margin-top: -35px" disabled>
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                <p style="margin-top: 15px; font-size: 13px">หน้าบัญชีธนาคาร</p>
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="doc2" style="margin-top: -35px" disabled>
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                <p style="margin-top: 15px; font-size: 13px">ใบรับรองแพทย์</p>
            </vs-checkbox>
            <vs-checkbox color="var(--main-color)" v-model="doc3" style="margin-top: -35px" disabled>
                <template #icon>
                    <i class='ti ti-check'></i>
                </template>
                <p style="margin-top: 15px; font-size: 13px">สำเนาบันทึกประจำวันของพนักงานสอบสวน</p>
            </vs-checkbox>

        </div>
        <div v-if="invoiceDoc">

            <br />
            <div class="box-container mt-2" v-for="(input, index) in bills" :key="`Bill-${index}`">
                <div class="input wrapper flex items-center">
                    <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                    <!--<input type="file" @change="onFileChange">-->
                    <file-pond credits="null"
                               label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                               v-bind:allow-multiple="false"
                               v-bind:allowFileEncode="true"
                               accepted-file-types="image/jpeg, image/png"
                               v-if="input.editImage"
                               v-bind:files="input.file"
                               v-model="input.file"
                               v-on:addfile="onAddBillFile(index)" />
                    <div class="mt-2 mb-2" v-if="!input.editImage" align="center">
                        <div class="div-center-image">
                            <div class="divImage">
                                <img class="img-show" :src="input.billFileShow" />
                                <br />
                                <label>{{input.filename}}</label>
                            </div>
                        </div>
                    </div>
                    <button class="btn-change-bank " @click="changeBillImage(index)">{{input.displayBtnChangeBillImage}}</button>
                    <vs-dialog width="550px" not-center v-model="modalHospital">
                        <template #header>
                            <h4 class="not-margin">
                                เลือกโรงพยาบาล
                            </h4>
                        </template>
                        <div class="con-content" align="left">
                            <div class="d-block text-left">
                                <div class="mb-2">
                                    <label class="px-2">จังหวัด</label>
                                    <select name="category" id="category" v-model="selectChangwat" @change="onChangwatChange">
                                        <option v-for="(category, index) in changwats" :value="category.changwatshortname" :key="index" style="font-size: 12px; line-height: 0px">
                                            {{ category.changwatname }}
                                        </option>
                                    </select>
                                </div>
                                <div class="mb-2" v-show="divHospitalModal">
                                    <label class="px-2">โรงพยาบาล</label>
                                    <select name="item" id="item" v-model="mockHospital">
                                        <option v-for="(item, index) in filteredItems" :value="item.HOSPITALNAME " :key="index">
                                            {{ item.HOSPITALNAME  }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <template #footer>
                            <div class="footer-dialog">
                                <vs-button block @click="submitModalHospital(index)">
                                    ยืนยัน
                                </vs-button>

                            </div>
                        </template>
                    </vs-dialog>
                    <vs-dialog width="550px" not-center v-model="modalWounded">
                        <template #header>
                            <h4 class="not-margin">
                                เลือกอาการบาดเจ็บ
                            </h4>
                        </template>
                        <div class="con-content" align="left">
                            <div class="d-block text-left">
                                <div class="mb-2">
                                    <label class="px-2">ประเภท</label>
                                    <select name="category" id="category" v-model="selectOrgan" @change="onOrganChange">
                                        <option v-for="(category, index) in organ" :value="category" :key="index" style="font-size: 12px; line-height: 0px">
                                            {{ category }}
                                        </option>
                                    </select>
                                </div>
                                <div class="mb-2" v-show="divWoundedModal">
                                    <label class="px-2">อาการบาดเจ็บ</label>
                                    <select name="item" id="item" v-model="mockWounded">
                                        <option v-for="(item, index) in filteredWoundedItems" :value="item.wounded" :key="index">
                                            {{ item.wounded  }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <template #footer>
                            <div class="footer-dialog">
                                <vs-button block @click="submitModalWounded(index)">
                                    ยืนยัน
                                </vs-button>

                            </div>
                        </template>
                    </vs-dialog>
                    <label class="px-2">อาการบาดเจ็บ</label>
                    <b-form-input class="mt-0 mb-2" v-model="input.injuri" placeholder="" type="text" @click="modalWounded=!modalWounded"></b-form-input>
                    <label class="px-2">ประเภทผู้ป่วย</label>
                    <br />
                    <div class="mt-1 mb-2" style="float: left;">
                        <vs-radio v-model="input.typePatient" val="IPD" style="float: left">
                            ผู้ป่วยใน (นอนพักรักษาตัวในโรงพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป)
                        </vs-radio>
                        <vs-radio v-model="input.typePatient" val="OPD" class="mt-1" style="float: left">
                            ผู้ป่วยนอก (ไม่ได้นอนพักรักษาตัวในโรงพยาบาล)
                        </vs-radio>

                    </div>
                    <label class="px-2">โรงพยาบาล</label>
                    <b-form-input class="mt-0 mb-2" v-model="input.selectHospital" type="text" @click="modalHospital=!modalHospital" />
                    <label class="px-2">เลขที่ใบเสร็จ</label>
                    <b-form-input class="mt-0 mb-2" v-model="input.bill_no" type="text" placeholder="" />
                    <label class="px-2">จำนวนเงิน</label>
                    <b-form-input class="mt-0 mb-2" v-model="input.money" type="number" placeholder="" @change="calMoney" />
                    <div class="row">
                        <div class="col-8">
                            <label class="px-2">วันที่เข้ารักษา</label>
                        </div>
                        <div class="col-4">
                            <label class="px-2">เวลา</label>
                        </div>
                    </div>

                    <div class="row">
                        <v-date-picker v-model="input.hospitalized_date" class="flex-grow col-8" locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                            <template v-slot="{ inputValue, inputEvents }">
                                <input class=" mt-0 mb-2 form-control "
                                       :value="inputValue"
                                       v-on="inputEvents"
                                       readonly />
                            </template>
                        </v-date-picker>
                        <v-date-picker v-model="input.hospitalized_time" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig" :dayPopover="{}">
                            <template v-slot="{ inputValue, inputEvents }">
                                <input class=" mt-0 mb-2 form-control "
                                       :value="inputValue"
                                       v-on="inputEvents"
                                       readonly />
                            </template>
                        </v-date-picker>
                    </div>

                    <div v-if="input.typePatient==='IPD'">
                        <div class="row">
                            <div class="col-8">
                                <label class="px-2">วันที่ออกจากโรงพยาบาล</label>
                            </div>
                            <div class="col-4">
                                <label class="px-2">เวลา</label>
                            </div>
                        </div>
                        <div class="row">
                            <v-date-picker v-model="input.out_hospital_date" class="flex-grow col-8" locale="th" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                <template v-slot="{ inputValue, inputEvents }">
                                    <input class=" mt-0 mb-2 form-control "
                                           :value="inputValue"
                                           v-on="inputEvents"
                                           readonly />
                                </template>
                            </v-date-picker>
                            <v-date-picker v-model="input.out_hospital_time" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig">
                                <template v-slot="{ inputValue, inputEvents }">
                                    <input class=" mt-0 mb-2 form-control "
                                           :value="inputValue"
                                           v-on="inputEvents"
                                           readonly />
                                </template>
                            </v-date-picker>
                        </div>
                        <!--<b-form-datepicker v-model="input.out_hospital_date"
                        selected-variant="primary"
                        label-selected=""
                        label-no-date-selected=""
                        :close-button="true"
                        :label-help="null"
                        label-close-button="ปิด"
                        :today-button="true"
                        label-today-button="เลือกวันปัจจุบัน"
                        :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                        size="sm"
                        class="mt-0 mb-2 " locale="th" placeholder=""></b-form-datepicker>-->
                    </div>

                    <br>
                    <!-- Add Svg Icon-->
                    <p style="color: green">
                        <svg @click="addField(input, bills)"
                             xmlns="http://www.w3.org/2000/svg"
                             viewBox="0 0 30 30"
                             width="30"
                             height="30"
                             class="ml-2 cursor-pointer">
                            <path fill="none" d="M0 0h24v24H0z" />
                            <path fill="green" d="M11 11V7h2v4h4v2h-4v4h-2v-4H7v-2h4zm1 11C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16z" />
                        </svg>เพิ่มใบเสร็จ
                    </p>
                    <!-- Remove Svg Icon-->
                    <svg v-show="bills.length > 1"
                         @click="removeField(index, bills)"
                         xmlns="http://www.w3.org/2000/svg"
                         viewBox="0 0 30 30"
                         width="30"
                         height="30"
                         class="ml-2 cursor-pointer mb-2"
                         style="margin-top: -10px;">
                        <path fill="none" d="M0 0h24v24H0z" />
                        <path fill="#EC4899" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z" />
                    </svg>

                </div>
            </div>
            <br>
            <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
            <br>
        </div>

        <div v-if="doc2 || doc3">
            <div align="left">
                <label>เอกสารประกอบคำร้องกรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
            </div>
            <div class="box-container">
                <div v-if="doc2">
                    <p class="mb-0">ใบรับรองแพทย์</p>
                    <div>
                        <file-pond name="idCardFile"
                                   ref="pond"
                                   label-idle="กดที่นี่เพื่ออัพโหลดใบรับรองแพทย์"
                                   credits="null"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="idCardFile"
                                   v-on:addfile="onAddIdCardFile" />
                    </div>
                </div>
                <div v-if="doc3">
                    <p class="mb-0">สำเนาบันทึกประจำวันของพนักงานสอบสวน</p>
                    <div>
                        <file-pond name="diaryFile"
                                   ref="pond"
                                   label-idle="กดที่นี่เพื่ออัพโหลดสำเนาบันทึกประจำวัน"
                                   credits="null"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="diaryFile"
                                   v-on:addfile="onAddDiaryFile" />
                    </div>
                </div>
            </div>
            <br />
        </div>

        <div v-if="accountDoc">
            
            
            <div class="box-container">
                <div align="left">
                    <label>หน้าบัญชีธนาคารรับเงิน</label>
                </div>
                <div class="form-group ">

                    <file-pond name="bankFile"
                               ref="pond"
                               credits="null"
                               label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร1"
                               v-bind:allow-multiple="false"
                               accepted-file-types="image/jpeg, image/png"
                               v-on:addfile="onAddBankAccountFile"
                               v-if="inputBank.editBankImage" />

                    <div class="mt-2 mb-2" v-if="!inputBank.editBankImage" align="center">
                        <div class="div-center-image">
                            <div class="divImage">
                                <img class="img-show" :src="bankFileDisplay.base64"/>
                                <br />
                                <label></label>
                            </div>
                        </div>
                    </div>

                </div>
                <button class="btn-change-bank" @click="changeAccountBank">{{inputBank.displayBtnChangeBankImage}}</button>

                <p class="mb-0 px-2">ชื่อธนาคาร</p>
                <div class="mb-2">
                    <select v-model="inputBank.accountBankName" style="font-size: 13px;">
                        <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                            {{ bankName.name }}
                        </option>
                    </select>
                </div>
                <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                <b-form-input class="mt-0 mb-2" v-model="inputBank.accountName" placeholder=""></b-form-input>
                <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                <b-form-input class="mt-0 mb-2 mb-4" v-model="inputBank.accountNumber" placeholder=""></b-form-input>
                <!--<div v-if="haslastDocumentReceive">

    <p class="mb-0 px-2">ชื่อธนาคาร</p>
    <div class="mb-2">-->
                <!--<b-form-input id="defaultSelectBank" class="mt-0 mb-2" v-model="displayBankName" :disabled="haslastDocumentReceive"></b-form-input>-->
                <!--<select id="defaultSelectBank" v-model="lastDocumentReceive.accountBankName" style="font-size: 13px;">
                <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                    {{ bankName.name }}
                </option>
            </select>
        </div>
        <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
        <b-form-input id="defaultInputAccountName" class="mt-0 mb-2" v-model="lastDocumentReceive.accountName"></b-form-input>
        <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
        <b-form-input id="defaultInputAccountNumber" class="mt-0 mb-2 mb-4" v-model="lastDocumentReceive.accountNumber"></b-form-input>

    </div>-->

                <br>
            </div>
        </div>

        <br />
        <br />
        <button class="btn-confirm-money" type="button" @click="showSwal">ส่งเอกสารเพิ่มเติม</button>
        <br>
        <br>
        <br>
    </div>
</template>

<script>
    import axios from 'axios'
    import * as moment from "moment/moment";
    //Your Javascript lives within the Script Tag
    export default {
        name: "AddDocument",
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
                msg: 'ดำเนินการต่อ',
                idCardFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                diaryFile: null,
                diaryFileDisplay: { file: null, filename: "", base64: "" },
                // ---Bill

                injuri: '',
                typePatient: 0,
                typeContect: 1,
                bills: [{
                    billNo: 1, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: ""
                    , out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: ""
                    , file: null, billFileShow: "", filename: "", editBillImage:"", editImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ"
                }],
                total_amount: 0,
                // --BookBank
                inputBank: { accountName: '', accountNumber: '', accountBankName: '', bankId: '', bankBase64String: '', bankFilename: '', editBankImage: false, displayBtnChangeBankImage: ''},
                bank: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.type,
                // bankData: this.$store.state.bankStateData,
                billsFile: null,
                bankFile: null,
                bankFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                dialogHospital: false,
                // Radio in Dialog
                picked: 1,
                //----Get Bank Name
                invoicehd: [],
                //----Get Bank Name
                bankNames: [],
                selectBank: {
                    bankName: ''
                },
                //----Get Last Document Receive
                //lastDocumentReceive: [],
                //haslastDocumentReceive: false,
                //isBtnChangAccountBankShow: false,
                //----Get Hospital Name
                hospitals: [],
                //selectHospital: '',
                //----Get Changwat Name
                changwats: [],
                selectChangwat: '',
                modalHospital: false,
                mockHospital: '',
                divHospitalModal: false,
                //----Get Wounded
                wounded: [],
                organ: [],
                mockWounded: '',
                modalWounded: false,
                selectWounded: '',
                selectOrgan: '',
                divWoundedModal: false,
                //----Get Wounded
                documentCheck: [],
                // HosApp
                hosData: this.$store.getters.hosAppGetter(this.$route.params.id, this.$route.params.appNo),
                // Document Checkbox
                invoiceDoc: false,
                doc2: false,
                doc3: false,
                accountDoc: false,


            };
        },
        methods: {
            onOrganChange() {

                this.divWoundedModal = true;

            },
            changeAccountBank() {
                console.log("ChangeBank")
                if (!this.inputBank.editBankImage) {
                    console.log("ChangeBank1", this.inputBank )

                    this.inputBank.editBankImage = true
                    this.inputBank.displayBtnChangeBankImage = "ใช้รูปบัญชีเดิม"
                } else {
                    this.inputBank.editBankImage = false
                    this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
                }

            },
            changeBillImage(index) {
                if (this.bills[index].editImage == false) {
                    this.bills[index].editImage = true
                    this.bills[index].displayBtnChangeBillImage = "ใช้รูปใบเสร็จเดิม"
                } else {
                    this.bills[index].editImage = false
                    this.bills[index].displayBtnChangeBillImage = "แก้ไขรูปใบเสร็จ"
                }

            },
            onAddIdCardFile: function (error, file) {
                this.idCardFileDisplay.file = file
                this.idCardFileDisplay.filename = file.filename
                this.idCardFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            onAddDiaryFile: function (error, file) {
                this.diaryFileDisplay.file = file
                this.diaryFileDisplay.filename = file.filename
                this.diaryFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            loadInvoicehd() {
                for (let i = 0; i < this.invoicehd.length; i++) {

                    for (let j = 0; j < this.hospitals.length; j++) {
                        if (this.hospitals[j].HOSPITALID == this.invoicehd[i].hosid) {
                            this.bills[i].selectHospital = this.hospitals[j].HOSPITALNAME
                            this.bills[i].selectHospitalId = this.hospitals[j].HOSPITALID;
                        }
                    }
                    for (let j = 0; j < this.wounded.length; j++) {
                        if (this.wounded[j].woundedId == parseInt(this.invoicehd[i].mainconsider)) {
                            console.log("mainconsi ", parseInt(this.invoicehd[i].mainconsider))
                            this.bills[i].injuri = this.wounded[j].wounded
                            this.bills[i].injuriId = this.wounded[j].woundedId;
                        }
                    }

                    console.log(this.bills[i].billFileShow)
                    this.bills[i].billNo = this.invoicehd[i].idInvhd;
                    this.bills[i].money = this.invoicehd[i].suminv;
                    this.bills[i].bill_no = this.invoicehd[i].receiptNo;
                    this.bills[i].typePatient = this.invoicehd[i].victimType
                    this.bills[i].hospitalized_date = moment(this.invoicehd[i].takendate).format("YYYY-MM-DD");
                    this.bills[i].hospitalized_time = this.invoicehd[i].takentime.replace('.', ':');
                    this.bills[i].out_hospital_date = moment(this.invoicehd[i].dispensedate).format("YYYY-MM-DD");
                    this.bills[i].out_hospital_time = this.invoicehd[i].dispensetime.replace('.', ':');
                    this.getBillFileFromECM(this.invoicehd[i].idInvhd);
                }
            },
            getInvoicehd() {
                console.log('getInvoicehd');
                var url = '/api/approval/Invoicehd/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.invoicehd = response.data;
                        console.log("invoicehd: ", response.data);
                        if (response.data.length > 1) {
                            this.addField(null, this.bills);
                        }
                        this.loadInvoicehd();
                        this.calMoney();
                        this.getChangwatNames();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getLastDocumentReceive() {
                console.log('getLastDocumentReceive');
                var url = '/api/Approval/LastDocumentReceive/{accNo}/{victimNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo);
                axios.get(url)
                    .then((response) => {
                        this.getBankFileFromECM()
                        if (response.data[0] != null) {
                            for (let i = 0; i < this.bankNames.length; i++) {
                                if (response.data[0].accountBankName == this.bankNames[i].bankCode) {
                                    this.inputBank.accountBankName = this.bankNames[i].name
                                    this.inputBank.bankId = this.bankNames[i].bankCode
                                    this.inputBank.accountName = response.data[0].accountName
                                    this.inputBank.accountNumber = response.data[0].accountNumber
                                    this.inputBank.editBankImage = false
                                    this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
                                    return true;
                                }
                            }
                        }
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getBankNames() {
                console.log('getBankNames');
                var url = '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        this.bankNames = response.data;
                        console.log(response.data);
                        this.getLastDocumentReceive();
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getHospitalNames() {
                console.log('getHospitalNames');
                var url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/api/Utility/Hospital";
                axios.post(url)
                    .then((response) => {
                        this.hospitals = response.data.data;
                        console.log(this.hospitals);
                        this.getWoundeds();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getChangwatNames() {
                console.log('getChangwatNames');
                var url = '/api/Master/Changwat';
                axios.get(url)
                    .then((response) => {
                        this.changwats = response.data;
                        console.log(response.data);
                        this.getBankNames();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getWoundeds() {
                console.log('getWoundeds');
                var url = '/api/Master/Wounded';
                axios.get(url)
                    .then((response) => {
                        this.wounded = response.data.woundedList;
                        this.organ = response.data.organ
                        console.log(response.data);

                        this.getInvoicehd();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getDocumentCheck() {
                console.log('getDocumentCheck');
                var url = '/api/Approval/DocumentCheck/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.documentCheck = response.data;
                        if (this.documentCheck != null) {
                            if (this.documentCheck.bookbankStatus == "ไม่ผ่าน") {
                                this.accountDoc = true;
                            }
                            if (this.documentCheck.invoiceStatus == "ไม่ผ่าน") {
                                this.invoiceDoc = true;
                            }
                        }

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getBillFileFromECM(idInvhd) {
                var url = '/api/Approval/DownloadFromECM'
                const body = {
                    SystemId: '02',
                    TemplateId: '03',
                    DocumentId: '01',
                    RefId: idInvhd + '|' + this.accData.accNo + '|' + this.accData.victimNo + '|' + this.$route.params.appNo,
                };
                axios.post(url, JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    for (let i = 0; i < this.invoicehd.length; i++) {
                        if (this.invoicehd[i].idInvhd == idInvhd) {
                            this.bills[i].billFileShow = 'data:image/png;base64,' + response.data
                        }
                    }
                }).catch(function (error) {
                    console.log(error);
                });

            },
            getBankFileFromECM() {
                var url = '/api/Approval/DownloadFromECM'
                const body = {
                    SystemId: '03',
                    TemplateId: '09',
                    DocumentId: '01',
                    RefId: this.accData.accNo + '|' + this.accData.victimNo + '|' + this.$route.params.appNo,
                };
                axios.post(url, JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    this.bankFileDisplay.base64 = 'data:image/png;base64,' + response.data;
                }).catch(function (error) {
                    console.log(error);
                });

            },
            onAddBankAccountFile: function (error, file) {
                this.bankFileDisplay.file = file
                this.inputBank.bankBase64String = file.getFileEncodeBase64String()
                this.inputBank.bankFilename = file.filename
            },
            onAddBillFile: function (index) {
                this.bills[index].filename = this.bills[index].file[0].filename
                this.bills[index].editBillImage = this.bills[index].file[0].getFileEncodeBase64String()
                console.log("add bill: ", this.bills[index])
            },
            //getIt: function () {
            //    console.log(this.$refs.pond.getFiles());
            //},
            onChangwatChange() {

                this.divHospitalModal = true;

            },
            submitModalHospital(index) {
                this.bills[index].selectHospital = this.mockHospital
                this.modalHospital = false
                this.selectChangwat = 0;
                this.mockHospital = 0;
                this.divHospitalModal = false;
            },
            submitModalWounded(index) {
                this.bills[index].injuri = this.mockWounded
                this.modalWounded = false
                this.selectOrgan = 0;
                this.mockWounded = 0;
                this.divWoundedModal = false;
            },
            submitModalSaysoHospital() {
                this.saysoHospital = this.mockSaysoHospital
                this.modalSaysoHospital = false
                this.selectChangwat = 0;
                this.mockSaysoHospital = 0;
                this.divHospitalModal = false;
            },
            calMoney() {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseInt(this.bills[i].money)
                }
                this.total_amount = sum

            },
            addField(value, fieldType) {
                var index = this.bills.length + 1
                fieldType.push({
                    billNo: index, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: "",
                    out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: "",
                    file: null, billFileShow: "", filename: "", editBillImage:"", editImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ"
                });
                this.calMoney()
                console.log("fieldType: ", fieldType)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                this.calMoney()
                console.log(this.bills)
            },

            showSwal() {
                this.$swal({
                    icon: 'success',
                    //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งเอกสารเพิ่มเติมแล้ว',
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'CheckStatus', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
        },
        async mounted() {
            await this.getHospitalNames();
            await this.getDocumentCheck();



            this.selectChangwat = 0;
            this.selectHospital = 0;
            console.log('accData', this.accData);
            console.log('hosData', this.hosData);
        },

        computed: {
            filteredItems: function () {
                return this.hospitals.filter(function (el) {
                    return el.CHANGWATSHORTNAME === this.selectChangwat;
                }, this);
            },
            filteredWoundedItems: function () {
                return this.wounded.filter(function (el) {
                    return el.organ === this.selectOrgan;
                }, this);
            }
        }
    };
</script>

<style>
</style>