<template>
    <div>
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <vs-dialog width="550px" not-center v-model="active">
            <template #header>
                <h4 class="not-margin">
                    คำรับรอง
                </h4>
            </template>
            <div class="con-content" align="left">
                <div class="d-block text-left">
                    <p>
                        ข้าพเจ้าผู้ยื่นคำร้องขอในนามของผู้ประสบภัย ขอให้ค้ารับรองว่า.-
                        <br>
                        1. ข้าพเจ้าหรือประสบภัย ไม่เคยรับหรือทำสัญญาว่าจะรับค่าเสียหายเบื้องต้นจากเจ้าของรถ หรือผู้หนึ่งผู้ใด
                        และยังไม่เคยรับหรือยื่นขอรับค่าเสียหายเบื้องต้นจากกองทุนทดแทนผู้ประสบภัย
                    </p>
                    <p>
                        2. ข้าพเจ้าหรือผู้ประสบภัย
                    </p>
                    <div style="margin-top: -10px;">
                        <vs-radio color="var(--main-color)" v-model="picked" val="1" class="mb-1" style="float: left">
                            ไม่เคย&emsp;&emsp;
                        </vs-radio>
                        <vs-radio color="#7d33ff" v-model="picked" val="2" style="float: left">
                            เคย
                        </vs-radio>
                    </div>
                    <br />
                    <div class="mt-0" v-if="picked==='2'">
                        <br />
                        <label>จำนวนเงิน</label>
                        <b-form-input class="mt-0 mb-2" v-model="saysoMoney" type="number" placeholder="" />
                        <label>ชื่อสถานพยาบาล</label>
                        <b-form-input class="mt-0 mb-2" v-model="saysoHospital" type="text" @click="modalSaysoHospital=!modalSaysoHospital" />
                        <!--Dialog Select Hospital-->
                        <vs-dialog width="550px" not-center v-model="modalSaysoHospital">
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
                                        <select name="item" id="item" v-model="mockSaysoHospital">
                                            <option v-for="(item, index) in filteredItems" :value="item.HOSPITALNAME " :key="index">
                                                {{ item.HOSPITALNAME  }}
                                            </option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <template #footer>
                                <div class="footer-dialog">
                                    <vs-button block @click="submitModalSaysoHospital()">
                                        ยืนยัน
                                    </vs-button>
                                </div>
                            </template>
                        </vs-dialog>
                    </div>
                    <br />
                    <p style="margin-top: -15px">
                        มอบอำนาจให้สถานพยาบาลขอรับค่าเสียหายเบื้องต้น
                    </p>
                    <p class="mt-3">
                        3. เมื่อข้าพเจ้าได้รับค่าเสียหายเบื้องต้นจากบริษัทประกันภัยครบถ้วนตามจำนวนที่กฎหมายกำหนดแล้ว
                        ข้าพเจ้าขอสัญญาว่า ข้าพเจ้าจะไม่ไปเรียกร้องค่าเสียหายเบื้องต้นจากเจ้าของรถ หรือมอบอำนาจให้บุคคลอื่น
                        หรือสถานพยาบาลมารับค่าเสียหายเบื้องต้นจำนวนนี้ซ้ำอีก
                    </p>
                    <div class="row">
                        <div class="col-2" style="padding-right: 0px; width:13%;">
                            <vs-checkbox color="#7d33ff" v-model="acceptData"></vs-checkbox>
                        </div>
                        <div class="col-10 px-0">
                            <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                                ข้าพเจ้าขอรับรองว่าข้อมูลดังกล่าวข้างต้นเป็นจริงทุกประการ หากข้าพเจ้าผิดคำรับรอง
                                ข้าพเจ้ายินยอมรับผิดในความเสียหายที่เกิดขึ้นทั้งหมดแก่บริษัท
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2" style="padding-right: 0px; width:13%;">
                            <vs-checkbox color="#7d33ff" v-model="acceptClaim"></vs-checkbox>
                        </div>
                        <div class="col-10 px-0">
                            <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                                ข้าพเจ้ารับทราบว่าผู้ประสบภัยผู้ใดยื่นคำร้องขอรับชดใช้ค่าเสียหายเบื้องต้น ตามพระราชบัญญัตินี้โดยทุจริต หรือแสดงหลักฐานอันเป็นเท็จ
                                เพื่อขอรับค่าเสียหายเบื้องต้นตามพระราชบัญญัตินี้ ต้องระวางโทษจำคุกไม่เกินห้าปี หรือปรับไม่เกินหนึ่งแสนบาท หรือทั้งจำทั้งปรับ
                            </p>
                        </div>
                    </div>
                    <div v-if="acceptClaim && acceptData" class="mb-4 mt-2" align="center">
                        <router-link class="btn-next" :to="{ name: 'ConfirmOTP', params: { id: accData.stringAccNo}}">ยืนยันส่งคำร้อง</router-link>
                    </div>
                </div>
            </div>
        </vs-dialog>
        <form-wizard title="" subtitle="" color="#5c2e91" step-size="xs" style="margin-top: -35px;" next-button-text="ดำเนินการต่อ" back-button-text="ย้อนกลับ" finish-button-text="ส่งคำร้อง" @on-complete="storeInputData">
            <tab-content title="สร้างคำร้อง" icon="ti ti-pencil-alt" :before-change="OnChangePageOne">

                <div class="" v-if="formType == 2">
                    <div align="left">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
                    </div>

                    <div class="box-container">
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
                    <br>
                    <!--<p class="p_right" style="font-size: 15px; font-weight: bold;">รวมจำนวนเงิน: {{ total_amount }} บาท</p>-->
                </div>
                <div class="" v-if="formType == 1">
                    <div align="left">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่ารักษาพยาบาลเบื้องต้น</label>
                    </div>

                    <div class="box-container mt-2" v-for="(input, index) in $v.bills.$each.$iter" :key="index">
                        <div class="input wrapper flex items-center">
                            <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                            <!--<input type="file" @change="onFileChange">-->
                            <file-pond credits="null"
                                       ref="pond"
                                       label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="bills[index].file"
                                       v-model="bills[index].file"
                                       v-on:addfile="onAddBillFile(index)" />
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
                            <b-form-input class="mt-0 mb-2" v-model="input.injuri.$model" type="text" @click="modalWounded=!modalWounded" :class="{ 'is-invalid': input.injuri.$error }"></b-form-input>
                            <div v-if="submitted && !input.injuri.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกอาการบาดเจ็บ</div>
                            <label class="px-2">ประเภทผู้ป่วย</label>
                            <br />
                            <div class="mt-1 mb-2" style="float: left;">
                                <vs-radio v-model="input.typePatient.$model" val="IPD" style="float: left">
                                    ผู้ป่วยใน (นอนพักรักษาตัวในโรงพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป)
                                </vs-radio>
                                <vs-radio v-model="input.typePatient.$model" val="OPD" class="mt-1" style="float: left">
                                    ผู้ป่วยนอก (ไม่ได้นอนพักรักษาตัวในโรงพยาบาล)
                                </vs-radio>

                            </div>
                            <label class="px-2">โรงพยาบาล</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.selectHospital.$model" type="text" @click="modalHospital=!modalHospital" :class="{ 'is-invalid': input.selectHospital.$error }" />
                            <div v-if="submitted && !input.selectHospital.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกโรงพยาบาล</div>
                            <div class="row">
                                <div class="col-6">
                                    <label class="px-2">ใบเสร็จเล่มที่</label>
                                    <b-form-input class="mt-0 mb-2 form-control" v-model.trim="input.bookNo.$model" type="text" :class="{ 'is-invalid': input.bookNo.$error }" />
                                    <div v-if="submitted && !input.bookNo.required" class="invalid-feedback" style="margin-top: -5px;">กรุณากรอกเล่มที่ใบเสร็จ</div>
                                </div>
                                <div class="col-6">
                                    <label class="px-2">เลขที่ใบเสร็จ</label>
                                    <b-form-input class="mt-0 mb-2 mb-0" v-model="input.bill_no.$model" type="text" :class="{ 'is-invalid': input.bill_no.$error }" />
                                    <div v-if="submitted && !input.bill_no.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่ใบเสร็จ</div>
                                </div>
                            </div>

                            <label class="px-2">จำนวนเงิน</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.money.$model" type="number" placeholder="" @change="calMoney" :class="{ 'is-invalid': input.money.$error }" />
                            <div v-if="submitted && !input.money.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกจำนวนเงิน</div>
                            <div class="row">
                                <div class="col-8">
                                    <label class="px-2">วันที่เข้ารักษา</label>
                                </div>
                                <div class="col-4">
                                    <label class="px-2">เวลา</label>
                                </div>
                            </div>

                            <div class="row">
                                <v-date-picker v-model="input.hospitalized_date.$model" class="flex-grow col-8" locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                    <template v-slot="{ inputValue, inputEvents }">
                                        <input class=" mt-0 mb-2 form-control "
                                               :value="inputValue"
                                               v-on="inputEvents"
                                               :class="{ 'is-invalid': input.hospitalized_date.$error }"
                                               readonly />
                                        <div v-if="submitted && !input.hospitalized_date.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกวันที่เข้ารักษา</div>
                                    </template>
                                </v-date-picker>

                                <v-date-picker v-model="input.hospitalized_time.$model" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig" :dayPopover="{}">
                                    <template v-slot="{ inputValue, inputEvents }">
                                        <input class=" mt-0 mb-2 form-control "
                                               :value="inputValue"
                                               v-on="inputEvents"
                                               :class="{ 'is-invalid': input.hospitalized_time.$error }"
                                               readonly />
                                        <div v-if="submitted && !input.hospitalized_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกเวลาที่เข้ารักษา</div>
                                    </template>
                                </v-date-picker>
                            </div>
                                                       
                            <div v-if="input.typePatient.$model === 'IPD'">
                                <div class="row">
                                    <div class="col-8">
                                        <label class="px-2">วันที่ออกจากโรงพยาบาล</label>
                                    </div>
                                    <div class="col-4">
                                        <label class="px-2">เวลา</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <v-date-picker v-model="input.out_hospital_date.$model" class="flex-grow col-8" locale="th" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                        <template v-slot="{ inputValue, inputEvents }">
                                            <input class=" mt-0 mb-2 form-control "
                                                   :value="inputValue"
                                                   v-on="inputEvents"
                                                   :class="{ 'is-invalid': input.out_hospital_date.$error }"
                                                   readonly />
                                            <div v-if="submitted && !input.out_hospital_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกวันที่ที่ออกจากโรงพยาบาล</div>
                                        </template>
                                    </v-date-picker>
                                    <v-date-picker v-model="input.out_hospital_time.$model" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig">
                                        <template v-slot="{ inputValue, inputEvents }">
                                            <input class=" mt-0 mb-2 form-control "
                                                   :value="inputValue"
                                                   v-on="inputEvents"
                                                   :class="{ 'is-invalid': input.out_hospital_time.$error }"
                                                   readonly />
                                            <div v-if="submitted && !input.out_hospital_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกเวลาที่ออกจากโรงพยาบาล</div>
                                        </template>
                                    </v-date-picker>
                                </div>
                      
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
                    <br>
                    <p class="p_right" style="font-size: 15px; font-weight: bold;">รวมจำนวนเงิน: {{ total_amount }} บาท</p>
                </div>
            </tab-content>
            <!-- บัญชีรับเงิน -->
            <tab-content title="บัญชีรับเงิน" icon="ti ti-money" :before-change="OnChangePageTwo">
                <div>
                    <p for="my-file" align="left" class="title-advice-menu mb-0">ภาพถ่ายหน้าสมุดบัญชีธนาคาร</p>

                    <div class="box-container mt-0">
                        <div class="form-group ">

                            <file-pond name="bankFile"
                                       ref="pond"
                                       credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร"
                                       v-bind:allow-multiple="false"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="bankFile"
                                       v-model="bankFile"
                                       v-on:addfile="onAddBankAccountFile"
                                       v-if="!haslastDocumentReceive" />
                            <!--<file-pond name="bankFile"
                                       ref="pond"
                                       credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร"
                                       v-bind:allow-multiple="false"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="bankFile"
                                       v-on:addfile="onAddBankAccountFile"
                                       v-if="haslastDocumentReceive" />-->
                            <div class="div-center-image" v-if="haslastDocumentReceive">
                                <div class="divImage">
                                    <img class="img-show" :src="bankFileDisplay.base64" />
                                    <br />
                                    <label class="lblFilename"></label>
                                </div>
                            </div>

                        </div>

                        <div class="mb-4" v-if="!haslastDocumentReceive">

                            <p class="mb-0 px-2">ชื่อธนาคาร</p>
                            <div class="mb-2">
                                <select v-model.trim="$v.inputBank.accountBankName.$model" style="font-size: 13px;" :class="{ 'is-invalid': $v.inputBank.accountBankName.$error }">
                                    <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                                        {{ bankName.name }}
                                    </option>
                                </select>
                                <div v-if="submitted && !$v.inputBank.accountBankName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกบัญชีธนาคาร</div>
                            </div>
                            <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model.trim="$v.inputBank.accountName.$model" :class="{ 'is-invalid': $v.inputBank.accountName.$error }" readonly></b-form-input>
                            <div v-if="submitted && !$v.inputBank.accountName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกชื่อบัญชีธนาคาร</div>
                            <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model.trim="$v.inputBank.accountNumber.$model" :class="{ 'is-invalid': $v.inputBank.accountNumber.$error }"></b-form-input>
                            <div v-if="submitted && !$v.inputBank.accountNumber.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่บัญชี</div>
                        </div>
                        <div class="mb-4" v-if="haslastDocumentReceive">

                            <p class="mb-0 px-2">ชื่อธนาคาร</p>
                            <div class="mb-2">
                                <!--<b-form-input id="defaultSelectBank" class="mt-0 mb-2" v-model="displayBankName" :disabled="haslastDocumentReceive"></b-form-input>-->
                                <select id="defaultSelectBank" v-model="lastDocumentReceive.accountBankName" style="font-size: 13px;" :disabled="haslastDocumentReceive">
                                    <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                                        {{ bankName.name }}
                                    </option>
                                </select>
                            </div>
                            <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                            <b-form-input id="defaultInputAccountName" class="mt-0 mb-2" v-model="lastDocumentReceive.accountName" :disabled="haslastDocumentReceive"></b-form-input>
                            <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                            <b-form-input id="defaultInputAccountNumber" class="mt-0 mb-2" v-model="lastDocumentReceive.accountNumber" :disabled="haslastDocumentReceive"></b-form-input>

                        </div>
                        <button v-if="isBtnChangAccountBankShow" class="btn-change-bank " @click="changeAccountBank">{{displayBtnChangeAccountBank}}</button>
                        <br>
                    </div>
                </div>
                <br>
            </tab-content>
            <!-- ผู้ประสบภัย -->
            <tab-content title="ส่งคำร้อง" icon="ti ti-share">
                <div align="left" style="width: 100%;">
                    <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลผู้ประสบภัย</label>
                </div>
                <div class="box-container mb-3">
                    <div class="row">
                        <div class="col-9">
                            <p class="mb-0">ชื่อ-สกุล</p>
                            <div class="mt-0" v-if="accidentVictimData.prefix != null">
                                <p class="label-text">{{accidentVictimData.prefix}}{{accidentVictimData.fname}} {{accidentVictimData.lname}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.prefix === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-3">
                            <p class="mb-0">อายุ</p>
                            <div class="mt-0" v-if="accidentVictimData.age != null">
                                <p class="label-text">{{accidentVictimData.age}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.age === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-4">
                            <p class="mb-0">บ้านเลขที่</p>
                            <div class="mt-0" v-if="accidentVictimData.homeId != null">
                                <p class="label-text">{{accidentVictimData.homeId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.homeId === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-2">
                            <p class="mb-0">หมู่</p>
                            <div class="mt-0" v-if="accidentVictimData.moo != null">
                                <p class="label-text">{{accidentVictimData.moo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.moo === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ซอย</p>
                            <div class="mt-0" v-if="accidentVictimData.soi != null">
                                <p class="label-text">{{accidentVictimData.soi}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.soi === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">ถนน</p>
                            <div class="mt-0" v-if="accidentVictimData.road != null">
                                <p class="label-text">{{accidentVictimData.road}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.road === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ตำบล/แขวง</p>
                            <div class="mt-0" v-if="accidentVictimData.tumbolName != null">
                                <p class="label-text">{{accidentVictimData.tumbolName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.tumbolName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">อำเภอ</p>
                            <div class="mt-0" v-if="accidentVictimData.districtName != null">
                                <p class="label-text">{{accidentVictimData.districtName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.districtName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">จังหวัด</p>
                            <div class="mt-0" v-if="accidentVictimData.provinceName != null">
                                <p class="label-text">{{accidentVictimData.provinceName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.provinceName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">รหัสไปรษณีย์</p>
                            <div class="mt-0" v-if="accidentVictimData.zipcode != null">
                                <p class="label-text">{{accidentVictimData.zipcode}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.zipcode === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">เบอร์โทรศัพท์</p>
                            <div class="mt-0" v-if="userData.mobileNo != null">
                                <p class="label-text">{{userData.mobileNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.mobileNo === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                </div>

                <!-- เหตุ -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลอุบัติเหตุ</label>
                </div>
                <div class="box-container mb-3">

                    <p class="mb-0">วันที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="accData.stringAccDate != null">
                        <p class="label-text">{{accData.stringAccDate}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accData.stringAccDate === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ลักษณะเกิดเหตุ</p>
                    <div class="mt-0" v-if="accData.accNature!=''">
                        <p class="label-text">{{accData.accNature}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accData.accNature===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">สถานที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="accData.placeAcc!=''">
                        <p class="label-text">{{accData.placeAcc}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accData.placeAcc===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <div class="row">
                        <div class="col-4">
                            <p class="mb-0">บ้านเลขที่ที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accHomeId != null">
                                <p class="label-text">{{accidentVictimData.accHomeId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accHomeId === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-2">
                            <p class="mb-0">หมู่ที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accMoo != null">
                                <p class="label-text">{{accidentVictimData.accMoo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accMoo === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ซอยที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accSoi != null">
                                <p class="label-text">{{accidentVictimData.accSoi}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accSoi === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">ถนนที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accRoad != null">
                                <p class="label-text">{{accidentVictimData.accRoad}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accRoad === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ตำบล/แขวงที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accTumbolName != null">
                                <p class="label-text">{{accidentVictimData.accTumbolName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accTumbolName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">อำเภอที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accDistrictName != null">
                                <p class="label-text">{{accidentVictimData.accDistrictName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accDistrictName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">จังหวัดที่เกิดเหตุ</p>
                            <div class="mt-0" v-if="accidentVictimData.accProvinceName != null">
                                <p class="label-text">{{accidentVictimData.accProvinceName}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="accidentVictimData.accProvinceName === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>

                    <p class="mb-0">หมายเลขทะเบียนรถคันเอาประกันภัย</p>
                    <div class="mt-0" v-if="accidentCarData.foundCarLicense != ''">
                        <p class="label-text">{{accidentCarData.foundCarLicense}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accidentCarData.foundCarLicense ===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">เลขตัวถัง</p>
                    <div class="mt-0" v-if="accidentCarData.foundChassisNo!=''">
                        <p class="label-text">{{accidentCarData.foundChassisNo}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accidentCarData.foundChassisNo===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                    <div class="mt-0" v-if="accidentCarData.foundPolicyNo!=''">
                        <p class="label-text">{{accidentCarData.foundPolicyNo}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accidentCarData.foundPolicyNo===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>

                </div>

                <!-- เอกสาร -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลเอกสารประกอบคำร้อง</label>
                </div>
                <div class="box-container mb-3">
                    <div class="card-bill" v-for="bill in bills" :key="bill.billNo">
                        <p class="mb-2">ใบเสร็จรับเงินค่ารักษาพยาบาลที่ {{bill.billNo}}</p>
                        <div class="div-center-image">
                            <div class="divImage" v-if="bill.billFileShow != null" align="center">
                                <img class="img-show" :src="bill.billFileShow" />
                                <br />
                                <label class="lblFilename">{{bill.filename}}</label>
                            </div>
                        </div>

                        <p class="mb-0">อาการบาดเจ็บ</p>
                        <div class="mt-0" v-if="bill.injuri!=''">
                            <p class="label-text">{{bill.injuri}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.injuri===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                        <p class="mb-0">ประเภทผู้ป่วย</p>
                        <div class="mt-0" v-if="bill.typePatient!=''">
                            <p class="label-text">{{bill.typePatient}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.typePatient===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                        <p class="mb-0">ชื่อโรงพยาบาล</p>
                        <div class="mt-0" v-if="bill.selectHospital!=''">
                            <p class="label-text">{{ bill.selectHospital }}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.selectHospital===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                        <div class="row">
                            <div class="col-7">
                                <p class="mb-0">วันที่เข้ารักษา</p>
                                <div class="mt-0" v-if="bill.hospitalized_date!=''">
                                    <p class="label-text">{{bill.hospitalized_date}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.hospitalized_date===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-5">
                                <p class="mb-0">เวลา</p>
                                <div class="mt-0" v-if="bill.hospitalized_time!=''">
                                    <p class="label-text">{{bill.hospitalized_time}} น.</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.hospitalized_time===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-7">
                                <p class="mb-0">วันที่ออกจากโรงพยาบาล</p>
                                <div class="mt-0" v-if="bill.out_hospital_date!=''">
                                    <p class="label-text">{{bill.out_hospital_date}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.out_hospital_date===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-5">
                                <p class="mb-0">เวลา</p>
                                <div class="mt-0" v-if="bill.out_hospital_time!=''">
                                    <p class="label-text">{{bill.out_hospital_time}} น.</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.out_hospital_time===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>

                        <!--<div class="row">
                            <div class="col-6">

                            </div>
                            <div class="col-6">

                            </div>
                        </div>-->
                        <div class="row">
                            <div class="col-7">
                                <p class="mb-0">เลขที่ใบเสร็จ</p>
                                <div class="mt-0" v-if="bill.bill_no!=''">
                                    <p class="label-text">{{bill.bill_no}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.bill_no===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-5">
                                <p class="mb-0">จำนวนเงิน</p>
                                <div class="mt-0" v-if="bill.money!=''">
                                    <p class="label-text">{{bill.money}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.money===''">
                                    <p class="label-text">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>

                    </div>
                    <p class="mb-0">จำนวนเงินรวมทั้งหมด</p>
                    <div class="mt-0" v-if="total_amount > 0">
                        <p class="label-text">{{total_amount}} บาท</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="total_amount <= 0">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                </div>
                <!-- บัญชี -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลบัญชีรับเงิน</label>
                </div>
                <div class="box-container mb-3">
                    <div v-if="!haslastDocumentReceive">
                        <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                        <div v-if="bankFileDisplay" align="center">
                            <div class="div-center-image">
                                <div class="divImage">
                                    <img class="img-show" :src="bankFileDisplay.base64" />
                                    <br />
                                    <label>{{bankFileDisplay.filename}}</label>
                                </div>
                            </div>

                        </div>

                        <p class="mb-0">ชื่อธนาคาร</p>
                        <div class="mt-0" v-if="inputBank.accountBankName!=''">
                            <p class="label-text">{{ inputBank.accountBankName }}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="inputBank.accountBankName===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>

                        <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                        <div class="mt-0" v-if="inputBank.accountName!=''">
                            <p class="label-text">{{inputBank.accountName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="inputBank.accountName===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                        <p class="mb-0">เลขบัญชีธนาคาร</p>
                        <div class="mt-0" v-if="inputBank.accountNumber!=''">
                            <p class="label-text">{{inputBank.accountNumber}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="inputBank.accountNumber===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div v-if="haslastDocumentReceive">
                        <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                        <div v-if="bankFileDisplay" align="center">
                            <div class="div-center-image">
                                <div class="divImage">
                                    <img class="img-show" :src="bankFileDisplay.base64" />
                                    <br />
                                    <label class="lblFilename">{{bankFileDisplay.filename}}</label>
                                </div>
                            </div>
                        </div>
                        <p class="mb-0">ชื่อธนาคาร</p>
                        <div class="mt-0" v-if="lastDocumentReceive.accountBankName!=''">
                            <p class="label-text">{{ lastDocumentReceive.accountBankName }}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="lastDocumentReceive.accountBankName===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>

                        <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                        <div class="mt-0" v-if="lastDocumentReceive.accountName!=''">
                            <p class="label-text">{{lastDocumentReceive.accountName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="lastDocumentReceive.accountName===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                        <p class="mb-0">เลขบัญชีธนาคาร</p>
                        <div class="mt-0" v-if="lastDocumentReceive.accountNumber!=''">
                            <p class="label-text">{{lastDocumentReceive.accountNumber}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="lastDocumentReceive.accountNumber===''">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>


                    <!-- <div class="form-check">
                    <input class="form-check-input" type="checkbox" v-model="acceptClaim">
                    <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                        ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                    </p>
                    </div> -->
                </div>
            </tab-content>

        </form-wizard>
    </div>
</template>

<script>
    import axios from 'axios'
    import { FormWizard, TabContent } from 'vue-form-wizard'
    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    import vueFilePond from 'vue-filepond';

    // Import FilePond styles
    import 'filepond/dist/filepond.min.css'

    // Import FilePond plugins
    // Please note that you need to install these plugins separately

    // Import image preview plugin styles
    import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

    // Import image preview and file type validation plugins
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    import { required } from "vuelidate/lib/validators";
    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode);


    /*import { required, minLength } from 'vuelidate/lib/validators';*/

    //Your Javascript lives within the Script Tag
    export default {
        name: "Claim",
        components: {
            FormWizard,
            TabContent,
            FilePond,
            Loading
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
                inputDataAll: [],
                // ---Bill
                patientType: '',
                bills: [{ billNo: 1, bill_no: "", bookNo: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: "", out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: "", file: null, billFileShow: "", filename: "" }],
                total_amount: 0,
                // --BookBank
                displayBankName: "",
                inputBank: { accountName: '', accountNumber: '', accountBankName: '', bankId: '', bankBase64String: '', bankFilename: '' },
                bank: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                preview: null,
                // ---Preview
                acceptClaim: false,
                acceptData: false,
                selected: 'first',
                options: [
                    { text: ' ไม่เคย', value: 'first' },
                    { text: ' เคย', value: 'second' },
                ],
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.type,
                // bankData: this.$store.state.bankStateData,
                idCardFile: null,
                billsFile: null,
                bankFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                bankFileDisplay: { file: null, filename: "", base64: "" },
                diaryFile: null,
                diaryFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                dialogHospital: false,
                // Radio in Dialog
                picked: 1,
                typePatient: 0,
                //----Get Bank Name
                bankNames: [],
                selectBank: {
                    bankName: ''
                },
                //----Get Last Document Receive
                base64FromECM: null,
                lastDocumentReceive: [],
                haslastDocumentReceive: false,
                isBtnChangAccountBankShow: false,
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
                //----Get AccidentCar
                accidentCarData: { foundCarLicense: '', foundChassisNo: '', foundPolicyNo: '' },
                //----Get AccidentCar
                accidentVictimData: {
                    accNo: null, victimNo: null, prefix: null, fname: null, lname: null, sex: null, age: null,
                    drvSocNo: null, homeId: null, moo: null, soi: null, road: null, tumbol: null, tumbolName: null,
                    district: null, districtName: null, province: null, provinceName: null, zipcode: null, telNo: null
                },
                //--- Say-so
                saysoMoney: '',
                saysoHospital: '',
                modalSaysoHospital: false,
                mockSaysoHospital: '',
                displayBtnChangeAccountBank: "",
                //--- loader
                isLoading: true,
                submitted: false


            };
        },
        validations() {
            if (this.bills.some(s => s.typePatient === "IPD")) {
                return {
                    bills: {
                        required,
                        $each: {
                            injuri: { required },
                            typePatient: { required },
                            selectHospital: { required },
                            bookNo: { required },
                            bill_no: { required },
                            money: { required },
                            hospitalized_date: { required },
                            hospitalized_time: { required },
                            out_hospital_date: { required },
                            out_hospital_time: { required } 

                        }
                    },
                    inputBank: {
                        accountName: { required },
                        accountNumber: { required },
                        accountBankName: { required }
                    },
                }

            } else {               
                return {
                    bills: {
                        required,
                        $each: {
                            injuri: { required },
                            typePatient: { required },
                            selectHospital: { required },
                            bookNo: { required },
                            bill_no: { required },
                            money: { required },
                            hospitalized_date: { required },
                            hospitalized_time: { required },
                            

                        }
                    },
                    inputBank: {
                        accountName: { required },
                        accountNumber: { required },
                        accountBankName: { required }
                    },
                }
            }
            

        },
        //---Validate
        //validations: {
        //    selectHospital: {
        //        required,
        //        minLength: minLength(5)
        //    }
        //},
        methods: {
            //---Validate
            /*status(validation) {
                return {
                    error: validation.$error,
                    dirty: validation.$dirty
                }
            },*/

            changeAccountBank() {
                if (this.haslastDocumentReceive == false) {
                    this.haslastDocumentReceive = true
                    this.displayBtnChangeAccountBank = "เปลี่ยนบัญชีรับเงิน"
                } else {
                    this.haslastDocumentReceive = false
                    this.displayBtnChangeAccountBank = "ใช้บัญชีเดิม"
                }

            },
            storeInputData() {
                for (let i = 0; i < this.bankNames.length; i++) {
                    if (this.inputBank.accountBankName == this.bankNames[i].name) {
                        this.inputBank.bankId = this.bankNames[i].bankCode
                    }
                }
                for (let j = 0; j < this.bills.length; j++) {
                    for (let i = 0; i < this.hospitals.length; i++) {
                        if (this.bills[j].selectHospital == this.hospitals[i].HOSPITALNAME) {
                            this.bills[j].selectHospitalId = this.hospitals[i].HOSPITALID
                        }
                    }
                }
                console.log("orgen", this.organ.length)
                for (let j = 0; j < this.bills.length; j++) {
                    for (let i = 0; i < this.wounded.length; i++) {
                        if (this.bills[j].injuri == this.wounded[i].wounded) {
                            this.bills[j].injuriId = this.wounded[i].woundedId
                        }
                    }
                }


                this.active = true

                this.$store.state.inputApprovalData.AccNo = this.accData.accNo
                this.$store.state.inputApprovalData.VictimNo = this.accData.victimNo
                this.$store.state.inputApprovalData.AppNo = this.accData.lastClaim.appNo
                this.$store.state.inputApprovalData.SumMoney = this.total_amount
                this.$store.state.inputApprovalData.ClaimNo = this.accData.lastClaim.claimNo
                this.$store.state.inputApprovalData.UserIdLine = this.$store.state.userTokenLine
                this.$store.state.inputApprovalData.Injury = this.injuri
                this.$store.state.inputApprovalData.BillsData = this.bills
                this.$store.state.inputApprovalData.BankData = (this.haslastDocumentReceive) ? this.lastDocumentReceive : this.inputBank
                this.$store.state.inputApprovalData.VictimData = this.accidentVictimData
                console.log("stored", this.$store.state.inputApprovalData);
            },
            getBankNames() {
                console.log('getBankNames');
                var url = '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        //this.$store.state.bankStateData = response.data;
                        //this.bankData = this.$store.state.bankStateData
                        //console.log(this.$store.state.bankStateData);
                        this.bankNames = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getLastDocumentReceive() {
                console.log('getHospitalNames');
                var url = '/api/Approval/LastDocumentReceive/{accNo}/{victimNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo);
                axios.get(url)
                    .then((response) => {
                        this.lastDocumentReceive = response.data[0]

                        if (response.data.length == 0) {
                            this.isBtnChangAccountBankShow = false;
                            this.displayBtnChangeAccountBank = "ใช้บัญชีเดิม";
                            this.lastDocumentReceive = null;
                            this.haslastDocumentReceive = false;
                            this.isLoading = false
                        } else if (this.lastDocumentReceive != null) {
                            this.isBtnChangAccountBankShow = true;
                            this.displayBtnChangeAccountBank = "เปลี่ยนบัญชีรับเงิน";
                            this.haslastDocumentReceive = true;
                            this.getFileFromECM()
                        }

                        console.log('last documenttttt: ', response.data.length);
                        console.log('last document: ', response.data[0]);
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
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getAccidentCar() {
                console.log('getAccidentCar');
                var url = '/api/Accident/Car/{accNo}/{channel}'.replace('{accNo}', this.accData.stringAccNo).replace('{channel}', this.accData.channel);
                axios.get(url)
                    .then((response) => {
                        this.accidentCarData = response.data;
                        console.log(this.accidentCarData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getAccidentVictim() {
                console.log('getAccidentVictim');
                var mockIdcard = this.userData.idcardNo /*'3149900145384'*/;
                var url = '/api/Accident/Victim/{accNo}/{ch}/{userIdCard}'.replace('{accNo}', this.accData.stringAccNo).replace('{ch}', this.accData.channel).replace('{userIdCard}', mockIdcard);
                axios.get(url)
                    .then((response) => {
                        this.accidentVictimData = response.data;
                        this.inputBank.accountName = this.accidentVictimData.fname + " " + this.accidentVictimData.lname
                        console.log(this.accidentVictimData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getFileFromECM() {
                var url = '/api/Approval/DownloadFromECM'
                const body = {
                    SystemId: '03',
                    TemplateId: '09',
                    DocumentId: '01',
                    RefId: this.lastDocumentReceive.appNo + '|' + this.accData.accNo + '|' + this.accData.victimNo,
                };
                axios.post(url, JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    this.lastDocumentReceive.bankBase64String = response.data;
                    this.bankFileDisplay.base64 = 'data:image/png;base64,' + response.data;
                    this.isLoading = false
                }).catch(function (error) {
                    console.log(error);
                });

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
            onAddBankAccountFile: function (error, file) {
                console.log(file)
                this.bankFileDisplay.file = file
                this.bankFileDisplay.filename = file.filename
                this.bankFileDisplay.base64 = file.getFileEncodeDataURL()
                this.inputBank.bankBase64String = file.getFileEncodeBase64String()
                this.inputBank.bankFilename = file.filename
            },
            onAddBillFile: function (index) {
                console.log("add bill: ", this.bills[index])
                this.bills[index].filename = this.bills[index].file[0].filename
                this.bills[index].billFileShow = this.bills[index].file[0].getFileEncodeDataURL()
                
            },
            OnChangePageOne() {
                this.submitted = true;

                // stop here if form is invalid
                this.$v.bills.$touch();
                if (this.$v.bills.$invalid) {
                        return false;
                }
                
                


                if (this.lastDocumentReceive != null) {
                    console.log('start page two  : ', this.lastDocumentReceive.accountBankName)
                    for (let i = 0; i < this.bankNames.length; i++) {
                        if (this.lastDocumentReceive.accountBankName == this.bankNames[i].bankCode) {
                            this.lastDocumentReceive.accountBankName = this.bankNames[i].name
                            this.lastDocumentReceive.bankId = this.bankNames[i].bankCode
                            console.log('before page two  : ', this.lastDocumentReceive.accountBankName)
                            this.submitted = false;
                            return true;
                        }
                    }
                }

                //for (let i = 0; i < this.bills.length; i++) {
                //    this.bills[i].BillfileShow = this.bills[i].file.getFileEncodeDataURL()
                //    this.bills[i].filename = this.bills[i].file.filename
                //}
                this.submitted = false;
                return true;
            },
            OnChangePageTwo() {
                this.submitted = true;

                
                // stop here if form is invalid
                if (!this.haslastDocumentReceive) {
                    this.$v.inputBank.$touch();
                    if (this.$v.inputBank.$invalid) {
                        return false;
                    }
                }
                


                if (this.lastDocumentReceive != null) {
                    
                    console.log('start page two  : ', this.lastDocumentReceive.accountBankName)
                    for (let i = 0; i < this.bankNames.length; i++) {
                        if (this.lastDocumentReceive.accountBankName == this.bankNames[i].bankCode) {
                            this.lastDocumentReceive.accountBankName = this.bankNames[i].name
                            console.log('before page two  : ', this.lastDocumentReceive.accountBankName)
                            return true;
                        }
                    }
                }

                return true;
            },
            //getIt: function () {
            //    console.log(this.$refs.pond.getFiles());
            //},
            onChangwatChange() {
                this.divHospitalModal = true;
            },
            onOrganChange() {
                this.divWoundedModal = true;
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
                fieldType.push({ billNo: index, bill_no: "", bookNo: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: "", out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: "", file: null, billFileShow: "", filename: "" });
                this.calMoney()
                console.log(this.bills)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                console.log(this.bills[index].file[0].id);
                /*this.$refs.pond.removeFile(this.bills[index].file[0].id);*/
                this.calMoney()
                console.log(this.bills)
            },
            //onFileChange(event) {
            //    var files = event.target.files || event.dataTransfer.files;
            //    if (!files.length)
            //        return;
            //    console.log(files[0]);
            //},
            //createImage(file) {
            //    var Image = new Image();
            //    var reader = new FileReader();
            //    var vm = this;

            //    reader.onload = (e) => {
            //        vm.image = e.target.result;
            //    };
            //    reader.readAsDataURL(file);
            //},
            //removeImage: function () {
            //    this.image = '';
            //},
            // --- BookBank
            //previewImage: function (event) {
            //    var input = event.target;
            //    if (input.files) {
            //        var reader = new FileReader();
            //        reader.onload = (e) => {
            //            this.preview = e.target.result;
            //            console.log(this.preview)
            //        }
            //        this.image = input.files[0];
            //        reader.readAsDataURL(input.files[0]);
            //    }
            //},
            //---Alert
            onComplete: function () {
                alert("Thank you for your response! We will contact you soon.");
            },
        },
        mounted() {

        },
        async created() {
            await this.getAccidentCar();
            await this.getAccidentVictim();
            this.getBankNames();
            this.getLastDocumentReceive();
            this.getHospitalNames();
            this.getChangwatNames();
            this.getWoundeds();
            this.selectChangwat = 0;
            this.selectHospital = 0;

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
    .invalid-feedback {
        margin-left:5px;
        margin-bottom:5px;
    }
    .div-center-image {
        text-align: -webkit-center;
    }

    .divImage {
        width: 75%;
        border-style: dashed;
        border-radius: 10px;
        border-color: #d9d9d9;
        background-color: #ffffff;
        border-width: thin;
    }

    .lblFilename {
        margin-bottom: 5px;
        font-size: 12px;
        border-radius: 5px;
        background-color: #f0f0f0;
    }

    span.vc-day {
        display: none;
    }

    span.vc-weekday {
        display: none;
    }

    span.vc-month {
        display: none;
    }

    span.vc-year {
        display: none;
    }

    .vue-form-wizard.xs .wizard-icon-circle {
        background-color: #f0f0f0;
    }

    .vue-form-wizard .wizard-icon-circle {
        border: 2.5px solid #bdbdbd;
        /*border: none;*/
    }

    .vue-form-wizard .wizard-nav-pills > li > a {
        color: #9c9c9c;
    }

    div.wizard-footer-left {
        float: left;
        background-color: transparent;
    }

    .vue-form-wizard .wizard-btn {
        border-radius: 10px;
        font-size: 13px;
    }

    .vue-form-wizard .wizard-icon-circle .wizard-icon-container {
        border-radius: 50%;
        padding: 20px 10px;
        margin-top: -2px;
    }

    .vue-form-wizard .wizard-btn .wizard-footer-left {
        color: red;
    }

    .vue-form-wizard .wizard-nav-pills a, .vue-form-wizard .wizard-nav-pills li {
        flex: 0px;
    }

    .filepond--drop-label label {
        font-family: 'Mitr';
        font-size: 14px;
    }

    .filepond--drop-label {
        border-style: outset;
        border-radius: 7px;
        border-color: #bdbdbd;
    }

    input[type=number] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    input[type=date] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    .b-form-btn-label-control.form-control {
        border: 2px solid #bbbbbb;
        border-radius: 4px;
    }

        .b-form-btn-label-control.form-control > .dropdown-menu {
            padding: 0.5rem;
            background-color: #f8f9fa;
        }

    .img-show {
        margin: 5px;
        max-height: 90px;
        border-radius: 10px;
    }

    .card-file {
        border: 1px dashed gray;
        opacity: 0.80;
        border-radius: 10px;
        padding: 5px;
        margin-bottom: 10px;
    }

    .card-bill {
        border: 1px solid #cccccc;
        border-radius: 10px;
        padding: 10px;
        margin-bottom: 10px;
    }

    #bv-modal {
        font-family: "Mitr";
    }

    .d-block {
        font-size: 14px;
        line-height: 20px;
    }

    .box-container {
        background-color: white;
        border: none;
    }
    /* Input */
    .mt-0.mb-2.form-control {
        background-color: #e1deec;
        border: none;
        border-radius: 7px;
        padding: 5px 10px;
        outline: none;
        font-size: 13px;
    }

        .mt-0.mb-2.form-control:hover {
            border: none;
            box-shadow: 1px 2px var(--main-color);
            transform: translateX(1px);
            outline: none;
        }

    select {
        width: 100%;
        background-color: #e1deec;
        border: none;
        border-radius: 7px;
        padding: 8px 10px;
        padding-right: 30px;
        outline: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        background-image: url("data:image/svg+xml;utf8,<svg fill='black' height='24' viewBox='0 0 24 24' width='24' xmlns='http://www.w3.org/2000/svg'><path d='M7 10l5 5 5-5z'/><path d='M0 0h24v24H0z' fill='none'/></svg>");
        background-repeat: no-repeat;
        background-position-x: 98%;
        background-position-y: 50%;
    }

        select:hover {
            border: none;
            box-shadow: 1px 2px var(--main-color);
            transform: translateX(1px);
            outline: none;
        }

    .label-text {
        margin-bottom: 0px;
        color: gray;
    }

    .btn-change-bank {
        background-color: white;
        color: #5c2e91;
        padding: 3px 15px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        border-style: solid;
        border-color: var(--main-color);
        font-weight: bold;
        font-size: 12px;
        margin-top: 10px;
        margin: 0 auto;
        display: block;
    }
</style>